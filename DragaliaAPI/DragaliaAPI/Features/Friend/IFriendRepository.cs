using DragaliaAPI.Database.Entities;

namespace DragaliaAPI.Features.Friend;

public interface IFriendRepository
{
    IQueryable<DbPlayerSupportChara> SupportChara { get; }

    Task<DbPlayerSupportChara?> GetSupportCharaAsync();
}
