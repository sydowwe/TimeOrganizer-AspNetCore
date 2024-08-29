using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class ActivityHistoryProfile : Profile
{
    public ActivityHistoryProfile()
    {
        CreateMap<ActivityHistoryRequest, ActivityHistory>();
        CreateMap<ActivityHistory, ActivityHistoryResponse>();
    }
}