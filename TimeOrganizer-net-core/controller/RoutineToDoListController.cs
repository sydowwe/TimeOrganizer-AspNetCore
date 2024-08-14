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
public class RoutineToDoListController(IRoutineToDoListService service)
    : AbstractWithActivityController<RoutineToDoList, RoutineToDoListRequest, RoutineToDoListResponse, IRoutineToDoListService>(service)
{
    [HttpPatch("change-done")]
    public async Task<ActionResult<SuccessResponse>> changeDone(List<IdRequest> requestList)
    {
        await service.setIsDoneAsync(requestList);
        return Ok(new SuccessResponse("changed"));
    }
    [NonAction]
    public override Task<ActionResult<IEnumerable<RoutineToDoListResponse>>> getAll()
    {
        return null;
    }
    [HttpPost("get-all")]
    public async Task<ActionResult<IEnumerable<RoutineToDoListGroupedResponse>>> getAllGrouped()
    {
        return Ok(await service.getAllGroupedByTimePeriod());
    }
}