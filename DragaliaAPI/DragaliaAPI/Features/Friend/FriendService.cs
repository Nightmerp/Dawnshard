using AutoMapper;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Models.Generated;
using DragaliaAPI.Shared.Definitions.Enums;

namespace DragaliaAPI.Features.Friend;

public class FriendService(IFriendRepository friendRepository, IMapper mapper) : IFriendService
{
    public async Task<SettingSupport> GetSupportChara()
    {
        DbPlayerSupportChara? dbSupportChara = await friendRepository.GetSupportCharaAsync();

        if (dbSupportChara == null)
        {
            return DefaultSupportCharacter;
        }
        else
        {
            return mapper.Map<SettingSupport>(dbSupportChara);
        }
    }

    private static readonly SettingSupport DefaultSupportCharacter =
        new(Charas.ThePrince, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
}
