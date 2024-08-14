using AutoMapper;
using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository;
using TimeOrganizer_net_core.service.abs;

namespace TimeOrganizer_net_core.service;

public interface ICategoryService : IMyService<Category, NameTextColorIconRequest, NameTextColorIconResponse>
{
}

public class CategoryService(ICategoryRepository repository, IUserService userService, IMapper mapper)
    : MyService<Category, NameTextColorIconRequest, NameTextColorIconResponse, ICategoryRepository>(repository, userService, mapper), ICategoryService;
