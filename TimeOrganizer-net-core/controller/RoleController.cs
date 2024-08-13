using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IRoleService service) : AbstractCrudController<Role,NameTextColorIconRequest,NameTextColorIconResponse,IRoleService>(service)
{
    [HttpPost("create-defaults")]
    public async Task<ActionResult<IEnumerable<NameTextColorIconResponse>>> createDefaults()
    {
        await service.createDefaultItems(0);
        return Ok("ok");
    }
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<NameTextColorIconResponse>>> getAll()
    {
        return await base.getAll();
    }
    [HttpPost("get/{id:long}")]
    public override async Task<ActionResult<NameTextColorIconResponse>> get(long id)
    {
        return await base.get(id);
    }
    // [HttpPost("get-by-name/{name}")]
    // public async Task<ActionResult<NameTextColorIconResponse>> GetByName(string name)
    // {
    //     var role = await _roleService.GetRoleByNameAsync(name);
    //     if (role == null)
    //         return NotFound();
    //     return Ok(role);
    // }

    [HttpPost("create")]
    public override async Task<ActionResult<NameTextColorIconResponse>> create(NameTextColorIconRequest newRole)
    {
        return await base.create(newRole);
    }
   
}