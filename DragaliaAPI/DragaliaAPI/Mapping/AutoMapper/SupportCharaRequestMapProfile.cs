using AutoMapper;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Models.Generated;

namespace DragaliaAPI.AutoMapper.Profiles;

public class SupportCharaRequestMapProfile : Profile
{
    public SupportCharaRequestMapProfile()
    {
        this.AddGlobalIgnore("ViewerId");
        this.AddGlobalIgnore("Owner");
        this.RecognizeDestinationPrefixes("Equip");

        this.CreateMap<FriendSetSupportCharaRequest, DbPlayerSupportChara>()
            .ForMember(x => x.CharaId, opts => opts.MapFrom(src => src.CharaId));
    }
}
