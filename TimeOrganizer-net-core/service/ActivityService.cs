using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IActivityService : IMyService<Activity, ActivityRequest, ActivityResponse>
{
  
}

public class ActivityService(IActivityRepository repository, IUserRepository userRepository, IMapper mapper)
    : MyService<Activity, ActivityRequest, ActivityResponse, IActivityRepository>(repository, userRepository, mapper),
        IActivityService
{
    
}
