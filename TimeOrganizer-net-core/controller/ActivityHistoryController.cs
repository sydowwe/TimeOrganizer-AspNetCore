using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.controller.extendable;
using TimeOrganizer_net_core.model.DTO.request.history;
using TimeOrganizer_net_core.model.DTO.response.history;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.service;

namespace TimeOrganizer_net_core.controller;

[ApiController]
[Route("[controller]")]
public class ActivityHistoryController(IActivityHistoryService service) : AbstractWithActivityController<ActivityHistory, ActivityHistoryRequest, ActivityHistoryResponse, IActivityHistoryService>(service)
{
}