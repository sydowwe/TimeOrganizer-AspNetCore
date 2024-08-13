using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class ActivityController(IActivityService service)
    : AbstractCrudController<Activity, ActivityRequest, ActivityResponse, IActivityService>(service)
{
    public override async Task<ActionResult<IEnumerable<ActivityResponse>>> getAll()
    {
        return await base.getAll();
    }

    [NonAction]
    public override async Task<ActionResult<ActivityResponse>> get(long id)
    {
        return await base.get(id);
    }

    
    public override async Task<ActionResult<ActivityResponse>> create(ActivityRequest toDoListRequest)
    {
        return await base.create(toDoListRequest);
    }

    
    public override async Task<ActionResult<ActivityResponse>> update(long id, ActivityRequest request)
    {
        return await base.update(id, request);
    }

    
    public override async Task<ActionResult<IdResponse>> delete(long id)
    {
        return await base.delete(id);
    }

    [NonAction]
    public override async Task<ActionResult<SuccessResponse>> batchDelete(List<IdRequest> request)
    {
        return await base.batchDelete(request);
    }
}