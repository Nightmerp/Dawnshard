using DragaliaAPI.Database;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Shared.PlayerDetails;
using Microsoft.EntityFrameworkCore;

namespace DragaliaAPI.Features.Friend;

public class FriendRepository(ApiContext apiContext, IPlayerIdentityService playerIdentityService)
    : IFriendRepository { }
