using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.activity;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class ActivityProfile: Profile
{
    public ActivityProfile()
    {
        CreateMap<ActivityRequest, Activity>();
        CreateMap<Activity, ActivityResponse>();
        CreateMap<Activity, ActivityFormSelectOptionsResponse>()
            .ForMember(dest=>dest.Label,opt=>opt.MapFrom(a=>a.Name))
            .ForMember(dest=>dest.RoleOption, opt=>opt.MapFrom(a=>a.Role))
            .ForMember(dest=>dest.CategoryOption, opt=>opt.MapFrom(a=>a.Category));
    }
}