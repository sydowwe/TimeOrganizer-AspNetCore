using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.activity;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class ActivityController(IActivityService service)
    : AbstractCrudController<Activity, ActivityRequest, ActivityResponse, IActivityService>(service)
{
    [NonAction]
    public override Task<ActionResult<IEnumerable<SelectOptionResponse>>> GetAllOptions()
    {
        return base.GetAllOptions();
    }
    [HttpPost("get-all-options")]
    public async Task<ActionResult<IEnumerable<ActivitySelectOptionResponse>>> GetAllActivityOptions()
    {
        return Ok(await service.GetAllAsOptionsAsync());
    }
}