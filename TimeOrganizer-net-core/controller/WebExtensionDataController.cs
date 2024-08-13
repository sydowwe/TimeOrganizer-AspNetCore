using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class WebExtensionDataController(IWebExtensionDataService service) : AbstractWithActivityController<WebExtensionData,WebExtensionDataRequest,WebExtensionDataResponse,IWebExtensionDataService>(service)
{
    [HttpPost("get-all")]
    public override async Task<ActionResult<IEnumerable<WebExtensionDataResponse>>> getAll()
    {
        return await base.getAll();
    }

    [HttpPost("create")]
    public override async Task<ActionResult<WebExtensionDataResponse>> create(WebExtensionDataRequest request)
    {
        return await base.create(request);
    }
}