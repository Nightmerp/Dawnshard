using DragaliaAPI.Database.Entities;

namespace DragaliaAPI.Features.Friend;

public interface IFriendRepository
{
    IQueryable<DbPlayerSupportChara> SupportChara { get; }

    IQueryable<DbFriend> Friends { get; }

    Task<DbPlayerSupportChara?> GetSupportCharaAsync();

    Task AddOrUpdateSupportCharaAsync(DbPlayerSupportChara supportChara);

    Task<DbFriend?> GetFriendAsync(long viewerID1, long viewerID2);

    Task AddOrUpdateFriend(DbFriend friend);

    public void RemoveFriend(DbFriend friend);
}
