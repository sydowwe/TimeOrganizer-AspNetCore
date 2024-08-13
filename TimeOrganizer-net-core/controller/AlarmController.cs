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
    [HttpGet("{id:long}")]
    public override async Task<ActionResult<AlarmResponse>> get(long id)
    {
        return await base.get(id);
    }

    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<AlarmResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpPost("create")]
    public override async Task<ActionResult<AlarmResponse>> create(AlarmRequest toDoListRequest)
    {
        return await base.create(toDoListRequest);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<AlarmResponse>> edit(long id, AlarmRequest request)
    {
        return await base.update(id, request);
    }

    [HttpDelete("{id:long}")]
    public override async Task<ActionResult<IdResponse>> delete(long id)
    {
        return await base.delete(id);
    }

    [HttpPost("batch-delete")]
    public override async Task<ActionResult<SuccessResponse>> batchDelete(List<IdRequest> request)
    {
        return await base.batchDelete(request);
    }
    [HttpPatch("change-active")]
    public async Task<ActionResult<SuccessResponse>> changeActive([FromBody] IEnumerable<IdRequest> requestList)
    {
        await service.setIsActive(requestList);
        return Ok(new SuccessResponse("changed"));
    }
}