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

    public IQueryable<DbFriend> Friends =>
        apiContext.Friends.Where(x =>
            x.ViewerId1 == playerIdentityService.ViewerId
            || x.ViewerId2 == playerIdentityService.ViewerId
        );

    public async Task<DbPlayerSupportChara?> GetSupportCharaAsync()
    {
        return await apiContext.PlayerSupportCharas.FindAsync(playerIdentityService.ViewerId);
    }

    public async Task AddOrUpdateSupportCharaAsync(DbPlayerSupportChara supportChara)
    {
        supportChara.ViewerId = playerIdentityService.ViewerId;
        DbPlayerSupportChara? dbSupportChara = await GetSupportCharaAsync();

        if (dbSupportChara == null)
        {
            await apiContext.PlayerSupportCharas.AddAsync(supportChara);
        }
        else
        {
            apiContext
                .PlayerSupportCharas.Entry(dbSupportChara)
                .CurrentValues.SetValues(supportChara);
        }
    }

    public async Task<DbFriend?> GetFriendAsync(long viewerID1, long viewerID2)
    {
        return await apiContext.Friends.FindAsync(viewerID1, viewerID2);
    }

    public async Task AddOrUpdateFriend(DbFriend friend)
    {
        DbFriend? dbFriend = await GetFriendAsync(friend.ViewerId1, friend.ViewerId2);

        if (dbFriend == null)
        {
            await apiContext.Friends.AddAsync(friend);
        }
        else
        {
            apiContext.Friends.Entry(dbFriend).CurrentValues.SetValues(friend);
        }
    }

    public void RemoveFriend(DbFriend friend)
    {
        apiContext.Friends.Remove(friend);
    }
}
