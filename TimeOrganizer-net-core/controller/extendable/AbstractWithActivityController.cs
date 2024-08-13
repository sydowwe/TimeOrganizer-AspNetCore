using Microsoft.AspNetCore.Mvc;
using TimeOrganizer_net_core.model.DTO.request.activity;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.controller.extendable;

public class AbstractWithActivityController<TEntity, TRequest, TResponse, TService>(TService service) : AbstractCrudController<TEntity, TRequest, TResponse, TService>(service) where TEntity : AbstractEntityWithActivity
    where TRequest : IActivityIdRequest
    where TResponse : IEntityWithActivityResponse
    where TService : IEntityWithActivityService<TEntity, TRequest, TResponse>
{
    
}