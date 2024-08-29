using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.controller.extendable;

public abstract class AbstractCrudController<TEntity, TRequest, TResponse, TService>(TService service) : ControllerBase
    where TEntity : AbstractEntity
    where TRequest : IRequest
    where TResponse : IIdResponse
    where TService : IMyService<TEntity, TRequest, TResponse>
{
    protected readonly TService service = service;

    [HttpPost("get-all")]
    public virtual async Task<ActionResult<IEnumerable<TResponse>>> getAll()
    {
        return Ok(await service.getAllAsync());
    }
    [HttpPost("get-all-options")]
    public virtual async Task<ActionResult<IEnumerable<TResponse>>> getAllOptions()
    {
        return Ok(await service.getAllAsOptionsAsync());
    }

    [HttpGet("{id:long}")]
    public virtual async Task<ActionResult<TResponse>> get(long id)
    {
        var response = await service.getByIdAsync(id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost("create")]
    public virtual async Task<ActionResult<TResponse>> create(TRequest request)
    {
        var newItem = await service.insertAsync(request);
        return CreatedAtAction(nameof(get), new { newItem.id }, newItem);
    }

    [HttpPut("{id:long}")]
    public virtual async Task<ActionResult<TResponse>> update(long id, TRequest request)
    {
        var updatedItem = await service.updateAsync(id, request);
        if (updatedItem == null)
            return NotFound();
        return Ok(updatedItem);
    }

    [HttpDelete("{id:long}")]
    public virtual async Task<ActionResult<IdResponse>> delete(long id)
    {
        try
        {
            await service.deleteAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
        return Ok(new IdResponse(id));
    }
    [HttpPost("batch-delete")]
    public virtual async Task<ActionResult<SuccessResponse>> batchDelete(List<IdRequest> request)
    {
        await service.batchDeleteAsync(request);
        return Ok(new SuccessResponse("deleted"));
    }
}

