using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IAlarmService : IEntityWithActivityService<Alarm, AlarmRequest, AlarmResponse>
{
    Task setIsActive(IEnumerable<IdRequest> requestList);
}

public class AlarmService(IAlarmRepository repository, ILoggedUserService loggedUserService, IMapper mapper)
    : MyService<Alarm, AlarmRequest, AlarmResponse, IAlarmRepository>(repository, loggedUserService, mapper), IAlarmService
{
    public async Task setIsActive(IEnumerable<IdRequest> requestList)
    {
        var ids = requestList.Select(req => req.id);
        var affectedRows = await Repository.updateIsActiveByIds(ids);
        if (affectedRows <= 0)
        {
            //throw new UpdateFailedException();
        }
    }
}