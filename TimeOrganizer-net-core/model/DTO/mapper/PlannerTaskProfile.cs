using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.plannerTask;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.mapper;

public class PlannerTaskProfile : Profile
{
    public PlannerTaskProfile()
    {
        CreateMap<PlannerTaskRequest, PlannerTask>();
        CreateMap<PlannerTask, PlannerTaskResponse>();
    }
}