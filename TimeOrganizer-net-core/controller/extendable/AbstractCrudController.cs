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
    public virtual async Task<ActionResult<IEnumerable<TResponse>>> GetAll()
    {
        return Ok(await service.GetAllAsync());
    }
    [HttpPost("get-all-options")]
    public virtual async Task<ActionResult<IEnumerable<TResponse>>> GetAllOptions()
    {
        return Ok(await service.GetAllAsOptionsAsync());
    }

    [HttpGet("{id:long}")]
    public virtual async Task<ActionResult<TResponse>> Get(long id)
    {
        var response = await service.GetByIdAsync(id);
        if (response == null)
            return NotFound();
        return Ok(response);
    }

    [HttpPost("create")]
    public virtual async Task<ActionResult<TResponse>> Create(TRequest request)
    {
        var newItem = await service.InsertAsync(request);
        return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
    }

    [HttpPut("{id:long}")]
    public virtual async Task<ActionResult<TResponse>> Update(long id, TRequest request)
    {
        var updatedItem = await service.UpdateAsync(id, request);
        if (updatedItem == null)
            return NotFound();
        return Ok(updatedItem);
    }

    [HttpDelete("{id:long}")]
    public virtual async Task<ActionResult<IdResponse>> Delete(long id)
    {
        try
        {
            await service.DeleteAsync(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return NotFound();
        }
        return Ok(new IdResponse(id));
    }
    [HttpPost("batch-delete")]
    public virtual async Task<ActionResult<SuccessResponse>> BatchDelete(List<IdRequest> request)
    {
        await service.BatchDeleteAsync(request);
        return Ok(new SuccessResponse("deleted"));
    }
}

