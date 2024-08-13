using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class CategoryController(ICategoryService service) : AbstractCrudController<Category,NameTextColorIconRequest,NameTextColorIconResponse,ICategoryService>(service)
{
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<NameTextColorIconResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpGet("{id:long}")]
    public override async Task<ActionResult<NameTextColorIconResponse>> get(long id)
    {
        return await base.get(id);
    }
    [HttpPost("create")]
    public override async Task<ActionResult<NameTextColorIconResponse>> create(NameTextColorIconRequest toDoListRequest)
    {
        return await base.create(toDoListRequest);
    }
    [HttpPut("{id:long}")]
    public async Task<ActionResult<NameTextColorIconResponse>> edit(long id, NameTextColorIconRequest request)
    {
        return await base.update(id,request);
    }
    [HttpDelete("{id:long}")]
    public override async Task<ActionResult<IdResponse>> delete(long id)
    {
        return await base.delete(id);
    }
    [HttpPost("batch-delete")]
    public override async Task<ActionResult<SuccessResponse>> batchDelete(List<IdRequest> request)
    {
        return await base.batchDelete(request);
    }
    
}