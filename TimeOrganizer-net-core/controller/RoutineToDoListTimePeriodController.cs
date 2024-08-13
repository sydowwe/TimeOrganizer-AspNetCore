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
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<TimePeriodResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpGet("{id:long}")]
    public override async Task<ActionResult<TimePeriodResponse>> get(long id)
    {
        return await base.get(id);
    }
    [HttpPut("{id:long}")]
    public async Task<ActionResult<TimePeriodResponse>> edit(long id, TimePeriodRequest request)
    {
        return await base.update(id,request);
    }
    [HttpPost("create-defaults")]
    public async Task<ActionResult<IEnumerable<TimePeriodResponse>>> createDefaults()
    {
        await service.createDefaultItems(0);
        return Ok("ok");
    }
    [HttpPost("change-is-hidden/{id:long}")]
    public async Task<ActionResult<SuccessResponse>> changeIsHidden(long id)
    {
        await service.changeIsHiddenInViewAsync(id);
        return Ok(new SuccessResponse("hidden changed"));
    }
}