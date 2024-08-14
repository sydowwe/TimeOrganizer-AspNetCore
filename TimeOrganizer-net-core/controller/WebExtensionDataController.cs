using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.generic;
using TimeOrganizer_net_core.model.DTO.request.ToDoList;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.DTO.response.toDoList;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class WebExtensionDataController(IWebExtensionDataService service) : AbstractWithActivityController<WebExtensionData,WebExtensionDataRequest,WebExtensionDataResponse,IWebExtensionDataService>(service)
{
    [NonAction]
    public override Task<ActionResult<WebExtensionDataResponse>> get(long id)
    {
        return null;
    }
    [NonAction]
    public override Task<ActionResult<WebExtensionDataResponse>> update(long id, WebExtensionDataRequest request)
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