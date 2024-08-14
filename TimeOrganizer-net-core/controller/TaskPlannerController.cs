using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.request.plannerTask;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class PlannerTaskController(IPlannerTaskService service)
    : AbstractWithActivityController<PlannerTask, PlannerTaskRequest, PlannerTaskResponse, IPlannerTaskService>(service)
{
   
    
    [HttpPatch("apply-filter")]
    public async Task<ActionResult<SuccessResponse>> applyFilter(PlannerFilterRequest request)
    {
        await service.getAllByDateAndHourSpan(request);
        return Ok(new SuccessResponse("changed"));
    }
    
    [HttpPatch("change-done")]
    public async Task<ActionResult<SuccessResponse>> changeDone(List<IdRequest> requestList)
    {
        await service.setIsDoneAsync(requestList);
        return Ok(new SuccessResponse("changed"));
    }
}