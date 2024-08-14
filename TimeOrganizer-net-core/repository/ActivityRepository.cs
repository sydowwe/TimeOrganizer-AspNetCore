using TimeOrganizer_net_core.model.entity;
using TimeOrganizer_net_core.repository.abs;

namespace TimeOrganizer_net_core.repository;

public interface IActivityRepository : IRepository<Activity>;
public class ActivityRepository(AppDbContext context) : ParentRepository<Activity>(context), IActivityRepository;