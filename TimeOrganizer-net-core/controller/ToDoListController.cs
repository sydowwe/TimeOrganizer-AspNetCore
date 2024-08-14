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
    [HttpPatch("change-done")]
    public async Task<ActionResult<SuccessResponse>> changeDone(List<IdRequest> requestList)
    {
        await service.setIsDoneAsync(requestList);
        return Ok(new SuccessResponse("changed"));
    }
}