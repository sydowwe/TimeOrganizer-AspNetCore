using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class WebExtensionDataProfile : Profile
{
    public WebExtensionDataProfile()
    {
        CreateMap<WebExtensionDataRequest, WebExtensionData>();
        CreateMap<WebExtensionData, WebExtensionDataResponse>();
    }
}