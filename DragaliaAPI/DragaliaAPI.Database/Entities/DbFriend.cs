using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DragaliaAPI.Database.Entities.Abstract;
using DragaliaAPI.Database.Utils;
using DragaliaAPI.Shared.Definitions.Enums;
using Microsoft.EntityFrameworkCore;

namespace DragaliaAPI.Database.Entities;

[PrimaryKey(nameof(ViewerId1), nameof(ViewerId2))]
public class DbFriend
{
    public DbPlayer? Player1 { get; set; }

    public DbPlayer? Player2 { get; set; }

    [ForeignKey(nameof(Player1))]
    public long ViewerId1 { get; set; }

    [ForeignKey(nameof(Player2))]
    [GreaterThan(nameof(ViewerId1))]
    public long ViewerId2 { get; set; }

    public FriendStatus FriendStatus { get; set; }

    public bool ViewerID1Viewed { get; set; }

    public bool ViewerID2Viewed { get; set; }
}
