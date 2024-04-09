using DragaliaAPI.Database.Entities;

namespace DragaliaAPI.Features.Friend;

public interface IFriendRepository
{
    IQueryable<DbPlayerSupportChara> SupportChara { get; }

    Task<DbPlayerSupportChara?> GetSupportCharaAsync();

    Task AddOrUpdateSupportCharaAsync(DbPlayerSupportChara supportChara);
}
