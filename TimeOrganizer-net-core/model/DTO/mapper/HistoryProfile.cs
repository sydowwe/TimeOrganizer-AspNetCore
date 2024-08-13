using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class HistoryProfile : Profile
{
    public HistoryProfile()
    {
        CreateMap<HistoryRequest, History>();
        CreateMap<History, HistoryResponse>();
    }
}