using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<NameTextColorIconRequest, Category>();
        CreateMap<Category, NameTextColorIconResponse>();
        CreateMap<Category, SelectOptionResponse>().ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name));
    }
}