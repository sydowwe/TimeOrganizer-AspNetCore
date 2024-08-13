using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class ToDoListController(IToDoListService service) : AbstractWithActivityController<ToDoList,ToDoListRequest,ToDoListResponse,IToDoListService>(service)
{
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<ToDoListResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpGet("{id:long}")]
    public override async Task<ActionResult<ToDoListResponse>> get(long id)
    {
        return await base.get(id);
    }
    [HttpPost("create")]
    public override async Task<ActionResult<ToDoListResponse>> create(ToDoListRequest toDoListRequest)
    {
        return await base.create(toDoListRequest);
    }
    [HttpPut("{id:long}")]
    public async Task<ActionResult<ToDoListResponse>> edit(long id, ToDoListRequest request)
    {
        return await base.update(id,request);
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
    [HttpPatch("change-done")]
    public async Task<ActionResult<SuccessResponse>> changeDone(List<IdRequest> requestList)
    {
        await service.setIsDoneAsync(requestList);
        return Ok(new SuccessResponse("changed"));
    }
}