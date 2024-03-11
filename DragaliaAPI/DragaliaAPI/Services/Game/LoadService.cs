﻿using System.Diagnostics;
using AutoMapper;
using DragaliaAPI.Database;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Features.Missions;
using DragaliaAPI.Features.Player;
using DragaliaAPI.Features.Present;
using DragaliaAPI.Features.Shop;
using DragaliaAPI.Features.Summoning;
using DragaliaAPI.Features.Trade;
using DragaliaAPI.Features.Wall;
using DragaliaAPI.Mapping.Mapperly;
using DragaliaAPI.Models.Generated;
using DragaliaAPI.Models.Options;
using DragaliaAPI.Shared.Definitions.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Riok.Mapperly.Abstractions;
using SummonTicketMapper = DragaliaAPI.Mapping.Mapperly.SummonTicketMapper;

namespace DragaliaAPI.Services.Game;

public class LoadService(
    ApiContext apiContext,
    IBonusService bonusService,
    ILogger<LoadService> logger,
    IOptionsMonitor<PhotonOptions> photonOptions,
    IMissionService missionService,
    IPresentService presentService,
    ITradeService tradeService,
    IUserService userService,
    TimeProvider timeProvider
) : ILoadService
{
    private static readonly DateTimeOffset QuestBonusStackBaseTime =
        new(2021, 04, 07, 06, 00, 00, TimeSpan.Zero); // 7. April 2017

    public async Task<LoadIndexResponse> BuildIndexData()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();

        Savefile savefile = await apiContext
            .Players.ProjectToSavefile()
            .AsSplitQuery()
            .AsNoTracking()
            .FirstAsync();

        logger.LogInformation("{time} ms: Load query complete", stopwatch.ElapsedMilliseconds);

        // TODO/NOTE: special shop purchase list is not set here. maybe change once that fully works?

        LoadIndexResponse data =
            new()
            {
                BuildList = savefile.BuildList,
                UserData = savefile.UserData,
                CharaList = savefile.CharaList,
                DragonList = savefile.DragonList,
                DragonReliabilityList = savefile.DragonReliabilityList,
                AbilityCrestList = savefile.AbilityCrestList,
                TalismanList = savefile.TalismanList,
                WeaponBodyList = savefile.WeaponBodyList,
                PartyList = savefile.PartyList,
                QuestList = savefile.QuestList,
                QuestEventList = savefile.QuestEvents,
                QuestTreasureList = savefile.QuestTreasureList,
                QuestWallList = savefile.QuestWalls,
                MaterialList = savefile.MaterialList,
                WeaponSkinList = savefile.WeaponSkinList,
                WeaponPassiveAbilityList = savefile.WeaponPassiveAbilityList,
                PartyPowerData = savefile.PartyPower,
                EquipStampList = savefile.EquippedStampList,
                SummonTicketList = savefile.SummonTickets,

                DragonGiftList = savefile.DragonGiftList.Where(x =>
                    x.DragonGiftId > DragonGifts.GoldenChalice
                ),
                QuestStoryList = savefile
                    .StoryStates.Where(x => x.StoryType == StoryTypes.Quest)
                    .Select(x => x.MapToQuestStoryList()),
                UnitStoryList = savefile
                    .StoryStates.Where(x => x.StoryType is StoryTypes.Chara or StoryTypes.Dragon)
                    .Select(x => x.MapToUnitStoryList()),
                CastleStoryList = savefile
                    .StoryStates.Where(x => x.StoryType == StoryTypes.Castle)
                    .Select(x => x.MapToCastleStoryList()),
                UserTreasureTradeList = savefile
                    .Trades.Where(x => x.Type == TradeType.Treasure)
                    .Select(x => x.MapToUserTreasureTradeList()),

                FriendNotice = new(0, 0),
                ShopNotice = new ShopNotice(savefile.ShopInfo?.DailySummonCount != 0),
                GuildNotice = new(0, false, false, false, false),
                StaminaMultiSystemMax = userService.StaminaMultiMax,
                StaminaMultiUserMax = 12,
                QuestBonusStackBaseTime = QuestBonusStackBaseTime,
                QuestSkipPointSystemMax = userService.QuestSkipPointMax,
                QuestSkipPointUseLimitMax = 30,
                QuestEntryConditionList = await missionService.GetEntryConditions(),
                MultiServer = new()
                {
                    Host = photonOptions.CurrentValue.ServerUrl,
                    AppId = string.Empty
                },
                ServerTime = timeProvider.GetUtcNow(),
                TreasureTradeAllList = tradeService.GetCurrentTreasureTradeList(),
                MissionNotice = await missionService.GetMissionNotice(null),
                FortBonusList = await bonusService.GetBonusList(),
                PresentNotice = await presentService.GetPresentNotice(),
                FunctionalMaintenanceList = [],
            };

        logger.LogInformation("{time} ms: Mapping complete", stopwatch.ElapsedMilliseconds);
        return data;
    }
}

public class Savefile
{
    public required UserData UserData { get; set; }

    public required PartyPowerData PartyPower { get; set; }

    public IEnumerable<PartyList> PartyList { get; set; } = [];

    public IEnumerable<CharaList> CharaList { get; set; } = [];

    public IEnumerable<DragonList> DragonList { get; set; } = [];

    public IEnumerable<QuestList> QuestList { get; set; } = [];

    public IEnumerable<QuestEventList> QuestEvents { get; set; } = [];

    public IEnumerable<MaterialList> MaterialList { get; set; } = [];

    public IEnumerable<WeaponSkinList> WeaponSkinList { get; set; } = [];

    public IEnumerable<WeaponBodyList> WeaponBodyList { get; set; } = [];

    public IEnumerable<WeaponPassiveAbilityList> WeaponPassiveAbilityList { get; set; } = [];

    public IEnumerable<AbilityCrestList> AbilityCrestList { get; set; } = [];

    public IEnumerable<TalismanList> TalismanList { get; set; } = [];

    public IEnumerable<DragonReliabilityList> DragonReliabilityList { get; set; } = [];

    public IEnumerable<DragonGiftList> DragonGiftList { get; set; } = [];

    public IEnumerable<EquipStampList> EquippedStampList { get; set; } = [];

    public IEnumerable<GenericStory> StoryStates { get; set; } = [];

    public IEnumerable<QuestTreasureList> QuestTreasureList { get; set; } = [];

    public IEnumerable<QuestWallList> QuestWalls { get; set; } = [];

    public IEnumerable<BuildList> BuildList { get; set; } = [];

    public IEnumerable<SummonTicketList> SummonTickets { get; set; } = [];

    public IEnumerable<GenericTrade> Trades { get; set; } = [];

    public DbPlayerShopInfo? ShopInfo { get; set; }
}

public class GenericStory
{
    public int StoryId { get; set; }

    public StoryTypes StoryType { get; set; }

    public StoryState State { get; set; }
}

public class GenericTrade
{
    public TradeType Type { get; set; }

    public required int Id { get; set; }

    public int Count { get; set; }

    public DateTimeOffset LastTradeTime { get; set; } = DateTimeOffset.UnixEpoch;
}

[Mapper(
    RequiredMappingStrategy = RequiredMappingStrategy.Target,
    IgnoreObsoleteMembersStrategy = IgnoreObsoleteMembersStrategy.Target
)]
public static partial class LoadMapper
{
    public static partial IQueryable<Savefile> ProjectToSavefile(this IQueryable<DbPlayer> query);

    public static UnitStoryList MapToUnitStoryList(this GenericStory story) =>
        new() { UnitStoryId = story.StoryId, IsRead = story.State == StoryState.Read, };

    public static QuestStoryList MapToQuestStoryList(this GenericStory story) =>
        new() { QuestStoryId = story.StoryId, State = story.State };

    public static CastleStoryList MapToCastleStoryList(this GenericStory story) =>
        new() { CastleStoryId = story.StoryId, IsRead = story.State == StoryState.Read };

    public static UserTreasureTradeList MapToUserTreasureTradeList(this GenericTrade trade) =>
        new()
        {
            TreasureTradeId = trade.Id,
            TradeCount = trade.Count,
            LastResetTime = trade.LastTradeTime
        };

    [MapProperty(nameof(DbPlayerDragonData.Level), nameof(DragonReliabilityList.ReliabilityLevel))]
    [MapProperty(nameof(DbPlayerDragonData.Exp), nameof(DragonReliabilityList.ReliabilityTotalExp))]
    private static partial DragonReliabilityList Map(this DbPlayerDragonReliability dbEntity);

    [MapProperty(nameof(DbParty.Units), nameof(PartyList.PartySettingList))]
    private static partial PartyList Map(this DbParty dbEntity);

    [MapperIgnoreTarget(nameof(CharaList.StatusPlusCount))]
    private static partial CharaList Map(DbPlayerCharaData playerCharaData);

    [MapperIgnoreTarget(nameof(UserData.AgeGroup))]
    [MapperIgnoreTarget(nameof(UserData.IsOptin))]
    [MapperIgnoreTarget(nameof(UserData.PrologueEndTime))]
    private static partial UserData Map(DbPlayerUserData userData);

    [MapperIgnoreTarget(nameof(DragonList.StatusPlusCount))]
    private static partial DragonList Map(DbPlayerDragonData dragonData);

    [MapProperty(nameof(DbAbilityCrest.AbilityLevel), nameof(AbilityCrestList.Ability1Level))]
    [MapProperty(nameof(DbAbilityCrest.AbilityLevel), nameof(AbilityCrestList.Ability2Level))]
    private static partial AbilityCrestList Map(DbAbilityCrest abilityCrest);

    private static partial WeaponBodyList Map(DbWeaponBody weaponBody);
}
