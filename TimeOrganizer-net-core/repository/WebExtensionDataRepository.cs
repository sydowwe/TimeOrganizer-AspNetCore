using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IWebExtensionDataRepository : IEntityWithActivityRepository<WebExtensionData>
{
}

public class WebExtensionDataRepository(AppDbContext context) : EntityWithActivityRepository<WebExtensionData>(context), IWebExtensionDataRepository;