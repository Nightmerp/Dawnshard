﻿using DragaliaAPI.Database.Entities;
using DragaliaAPI.Database.Utils;
using DragaliaAPI.Shared;
using DragaliaAPI.Shared.PlayerDetails;
using Microsoft.Extensions.Logging;

namespace DragaliaAPI.Database.Repositories;

/// <summary>
/// Provides methods to interface with the UserData table.
/// </summary>
public class UserDataRepository : BaseRepository, IUserDataRepository
{
    private readonly ApiContext apiContext;
    private readonly IPlayerDetailsService playerDetailsService;
    private readonly ILogger<UserDataRepository> logger;

    public UserDataRepository(
        ApiContext apiContext,
        IPlayerDetailsService playerDetailsService,
        ILogger<UserDataRepository> logger
    )
        : base(apiContext)
    {
        this.apiContext = apiContext;
        this.playerDetailsService = playerDetailsService;
        this.logger = logger;
    }

    public IQueryable<DbPlayerUserData> GetUserData(string deviceAccountId)
    {
        IQueryable<DbPlayerUserData> infoQuery = apiContext.PlayerUserData.Where(
            x => x.DeviceAccountId == deviceAccountId
        );

        return infoQuery;
    }

    public IQueryable<DbPlayerUserData> GetUserData(long viewerId)
    {
        return apiContext.PlayerUserData.Where(x => x.ViewerId == viewerId);
    }

    public async Task<ISet<int>> GetTutorialFlags(string deviceAccountId)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        int flags = userData.TutorialFlag;
        return TutorialFlagUtil.ConvertIntToFlagIntList(flags);
    }

    public async Task UpdateName(string deviceAccountId, string newName)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        userData.Name = newName;
    }

    public async Task SetTutorialFlags(string deviceAccountId, ISet<int> tutorialFlags)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        userData.TutorialFlag = TutorialFlagUtil.ConvertFlagIntListToInt(
            tutorialFlags,
            userData.TutorialFlag
        );
    }

    public async Task SetMainPartyNo(string deviceAccountId, int partyNo)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        userData.MainPartyNo = partyNo;
    }

    public async Task SkipTutorial()
    {
        DbPlayerUserData userData = await this.LookupUserData();

        userData.TutorialStatus = 60999;
        userData.TutorialFlagList = Enumerable.Range(1, 30).Select(x => x + 1000).ToHashSet();
    }

    public async Task UpdateSaveImportTime(string deviceAccountId)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        userData.LastSaveImportTime = DateTimeOffset.UtcNow;
    }

    [Obsolete(ObsoleteReasons.UsePlayerDetailsService)]
    public async Task GiveWyrmite(string deviceAccountId, int quantity)
    {
        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        userData.Crystal += quantity;
    }

    public async Task GiveWyrmite(int quantity)
    {
        DbPlayerUserData userData = await this.LookupUserData();

        userData.Crystal += quantity;
    }

    [Obsolete(ObsoleteReasons.UsePlayerDetailsService)]
    public async Task UpdateCoin(string deviceAccountId, long offset)
    {
        this.logger.LogDebug("Updating player rupies by {offset}", offset);
        if (offset == 0)
            return;

        DbPlayerUserData userData = await this.LookupUserData(deviceAccountId);

        long newQuantity = userData.Coin + offset; // changed from += to + bc otherwise it adds to quantity anyways which i don't
        if (newQuantity < 0) // think was what was intended since it renders last line useless
            throw new ArgumentException("Player cannot have negative rupies");

        userData.Coin = newQuantity;
    }

    public async Task UpdateCoin(long offset)
    {
#pragma warning disable CS0618
        await this.UpdateCoin(this.playerDetailsService.AccountId, offset);
#pragma warning restore CS0618
    }

    public async Task<bool> CheckCoin(long quantity)
    {
        long coin = (await this.LookupUserData()).Coin;
        bool result = coin >= quantity;

        if (!result)
        {
            this.logger.LogWarning(
                "Failed rupie check: requested {quantity} rupies, but user had {coin}",
                quantity,
                coin
            );
        }

        return result;
    }

    [Obsolete(ObsoleteReasons.UsePlayerDetailsService)]
    public async Task<DbPlayerUserData> LookupUserData(string deviceAccountId)
    {
        return await apiContext.PlayerUserData.FindAsync(deviceAccountId)
            ?? throw new NullReferenceException("Savefile lookup failed");
    }

    public async Task<DbPlayerUserData> LookupUserData() =>
#pragma warning disable CS0618
        await this.LookupUserData(this.playerDetailsService.AccountId);
#pragma warning restore CS0618

    public async Task UpdateDewpoint(int quantity)
    {
        if (quantity == 0)
            return;

        DbPlayerUserData userData = await this.LookupUserData();

        int newQuantity = userData.DewPoint + quantity;
        if (newQuantity < 0)
            throw new ArgumentException("Player cannot have negative eldwater");

        userData.DewPoint = newQuantity;
    }

    public async Task<bool> CheckDewpoint(int quantity)
    {
        int dewpoint = (await this.LookupUserData()).DewPoint;
        return dewpoint >= quantity;
    }

    public async Task SetDewpoint(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Player cannot have negative eldwater");

        DbPlayerUserData userData = await this.LookupUserData();
        userData.DewPoint = quantity;
    }
}
