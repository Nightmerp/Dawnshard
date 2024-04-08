using DragaliaAPI.Database;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Shared.PlayerDetails;
using Microsoft.EntityFrameworkCore;

namespace DragaliaAPI.Features.Friend;

public class FriendRepository(ApiContext apiContext, IPlayerIdentityService playerIdentityService)
    : IFriendRepository
{
    public IQueryable<DbPlayerSupportChara> SupportChara =>
        apiContext.PlayerSupportCharas.Where(x => x.ViewerId == playerIdentityService.ViewerId);

    public async Task<DbPlayerSupportChara?> GetSupportCharaAsync()
    {
        return await apiContext.PlayerSupportCharas.FindAsync(playerIdentityService.ViewerId);
    }
}
