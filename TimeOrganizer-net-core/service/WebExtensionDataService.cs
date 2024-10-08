using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.response;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.security;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface IWebExtensionDataService : IEntityWithActivityService<WebExtensionData, WebExtensionDataRequest, WebExtensionDataResponse>
{
}

public class WebExtensionDataService(IWebExtensionDataRepository repository, IActivityService activityService, ILoggedUserService loggedUserService, IMapper mapper)
    : EntityWithActivityService<WebExtensionData, WebExtensionDataRequest, WebExtensionDataResponse, IWebExtensionDataRepository>(repository, activityService, loggedUserService, mapper), IWebExtensionDataService;