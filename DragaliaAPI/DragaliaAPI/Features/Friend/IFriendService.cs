using DragaliaAPI.Database.Entities;
using DragaliaAPI.Models.Generated;

namespace DragaliaAPI.Features.Friend;

public interface IFriendService
{
    Task<SettingSupport> GetSupportChara();
}
