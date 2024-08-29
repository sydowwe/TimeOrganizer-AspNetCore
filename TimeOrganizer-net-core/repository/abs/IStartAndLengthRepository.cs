using TimeOrganizer_net_core.model.DTO.request;
using TimeOrganizer_net_core.model.DTO.response.generic;
using TimeOrganizer_net_core.model.entity.abs;

namespace TimeOrganizer_net_core.repository.abs;

public interface IStartAndLengthRepository<out TEntity>
    where TEntity : AbstractEntity
{
    Task<IEnumerable<AbstractEntity>> GetAllByDateAndHourSpan(int userId, DateTime startDate, DateTime endDate);
}