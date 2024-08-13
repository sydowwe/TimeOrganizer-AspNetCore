using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class TaskUrgencyController(ITaskUrgencyService service) : AbstractCrudController<TaskUrgency,TaskUrgencyRequest,TaskUrgencyResponse,ITaskUrgencyService>(service)
{
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<TaskUrgencyResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpGet("{id:long}")]
    public override async Task<ActionResult<TaskUrgencyResponse>> get(long id)
    {       
        return await base.get(id);
    }
}