using DragaliaAPI.Database;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Shared.PlayerDetails;
using Microsoft.EntityFrameworkCore;

namespace DragaliaAPI.Features.Friend;

public class SupportCharacterRepository(
    ApiContext apiContext,
    IPlayerIdentityService playerIdentityService
) : ISupportCharacterRepository { }
