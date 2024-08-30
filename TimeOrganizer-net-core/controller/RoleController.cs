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
    public async Task<ActionResult<IEnumerable<NameTextColorIconResponse>>> CreateDefaults()
    {
        await service.CreateDefaultItems(0);
        return Ok("ok");
    }
    [HttpPost("get-by-name/{name}")]
    public async Task<ActionResult<NameTextColorIconResponse>> GetByName([FromRoute] string name)
    {
        return Ok(await service.GetByNameAsync(name));
    }
   
    [NonAction]
    public override Task<ActionResult<NameTextColorIconResponse>> Update(long id, NameTextColorIconRequest request)
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