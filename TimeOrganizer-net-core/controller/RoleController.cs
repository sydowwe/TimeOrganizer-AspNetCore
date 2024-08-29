using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class RoleController(IRoleService service) : AbstractCrudController<Role,NameTextColorIconRequest,NameTextColorIconResponse,IRoleService>(service)
{
    [HttpPost("create-defaults")]
    public async Task<ActionResult<IEnumerable<NameTextColorIconResponse>>> createDefaults()
    {
        await service.createDefaultItems(0);
        return Ok("ok");
    }
   
    [NonAction]
    public override Task<ActionResult<NameTextColorIconResponse>> update(long id, NameTextColorIconRequest request)
    {
        return null;
    }
    [NonAction]
    public override  Task<ActionResult<IdResponse>> delete(long id)
    {
        return null;
    }
    [NonAction]
    public override Task<ActionResult<SuccessResponse>> batchDelete(List<IdRequest> request)
    {
        return null;
    }
   
}