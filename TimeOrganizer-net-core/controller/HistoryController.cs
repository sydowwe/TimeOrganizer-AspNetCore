using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class HistoryController(IHistoryService service) : AbstractWithActivityController<History, HistoryRequest, HistoryResponse, IHistoryService>(service)
{
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<HistoryResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpGet("{id:long}")]
    public override async Task<ActionResult<HistoryResponse>> get(long id)
    {
        return await base.get(id);
    }

    [HttpPost("create")]
    public override async Task<ActionResult<HistoryResponse>> create(HistoryRequest toDoListRequest)
    {
        return await base.create(toDoListRequest);
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<HistoryResponse>> edit(long id, HistoryRequest request)
    {
        return await base.update(id, request);
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