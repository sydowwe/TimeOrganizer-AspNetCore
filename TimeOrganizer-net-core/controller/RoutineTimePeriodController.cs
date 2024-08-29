using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class RoutineTimePeriodController(IRoutineTimePeriodService service) : AbstractCrudController<RoutineTimePeriod,TimePeriodRequest,TimePeriodResponse,IRoutineTimePeriodService>(service)
{
    [HttpPost("create-defaults")]
    public async Task<ActionResult<IEnumerable<TimePeriodResponse>>> CreateDefaults()
    {
        await service.CreateDefaultItems(0);
        return Ok("ok");
    }
    [HttpPost("change-is-hidden/{id:long}")]
    public async Task<ActionResult<SuccessResponse>> ChangeIsHidden(long id)
    {
        await service.ChangeIsHiddenInViewAsync(id);
        return Ok(new SuccessResponse("hidden changed"));
    }
}