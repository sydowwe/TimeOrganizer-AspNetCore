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
public class TaskUrgencyController(ITaskUrgencyService service) : AbstractCrudController<TaskUrgency,TaskUrgencyRequest,TaskUrgencyResponse,ITaskUrgencyService>(service)
{
    [NonAction]
    public override Task<ActionResult<TaskUrgencyResponse>> Create(TaskUrgencyRequest request)
    {
        return null;
    }
    [NonAction]
    public override Task<ActionResult<TaskUrgencyResponse>> Update(long id, TaskUrgencyRequest request)
    {
        return null;
    }
    [NonAction]
    public override  Task<ActionResult<IdResponse>> Delete(long id)
    {
        return null;
    }
    [NonAction]
    public override Task<ActionResult<SuccessResponse>> BatchDelete(List<IdRequest> request)
    {
        return null;
    }
}