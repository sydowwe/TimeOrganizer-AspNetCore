using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class AlarmController(IAlarmService service) : AbstractWithActivityController<Alarm, AlarmRequest, AlarmResponse, IAlarmService>(service)
{
    [HttpPatch("change-active")]
    public async Task<ActionResult<SuccessResponse>> changeActive([FromBody] IEnumerable<IdRequest> requestList)
    {
        await service.setIsActive(requestList);
        return Ok(new SuccessResponse("changed"));
    }
}