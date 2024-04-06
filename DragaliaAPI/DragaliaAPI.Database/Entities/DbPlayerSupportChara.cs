using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using DragaliaAPI.Database.Entities.Abstract;
using DragaliaAPI.Shared.Definitions.Enums;
using DragaliaAPI.Shared.Features.Summoning;
using DragaliaAPI.Shared.MasterAsset;
using DragaliaAPI.Shared.MasterAsset.Models;
using Microsoft.EntityFrameworkCore;

namespace DragaliaAPI.Database.Entities;

[Table("PlayerSupportChara")]
[PrimaryKey(nameof(ViewerId))]
public class DbPlayerSupportChara : DbUnitBase, IDbPlayerData
{
    public virtual DbPlayer? Owner { get; set; }

    [ForeignKey(nameof(Owner))]
    public long ViewerId { get; set; }
}
