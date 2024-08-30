using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<NameTextColorIconRequest, Role>();
        CreateMap<Role, NameTextColorIconResponse>();
        CreateMap<Role, SelectOptionResponse>().ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name));
    }
}