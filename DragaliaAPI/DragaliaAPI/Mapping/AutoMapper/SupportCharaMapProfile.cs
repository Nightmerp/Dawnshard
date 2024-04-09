using AutoMapper;
using DragaliaAPI.Database.Entities;
using DragaliaAPI.Models.Generated;

namespace DragaliaAPI.AutoMapper.Profiles;

public class SupportCharaMapProfile : Profile
{
    public SupportCharaMapProfile()
    {
        this.CreateMap<DbPlayerSupportChara, SettingSupport>();
    }
}
