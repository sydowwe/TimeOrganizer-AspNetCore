using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IActivityRepository : IEntityWithUserRepository<Activity>;
public class ActivityRepository(AppDbContext context) : EntityWithUserRepository<Activity>(context), IActivityRepository;