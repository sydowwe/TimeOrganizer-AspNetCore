using System.ComponentModel.DataAnnotations;
using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.activity;

public interface IActivityIdRequest : IRequest
{
    public long ActivityId { get; set; }
}
public class ActivityIdRequest : IActivityIdRequest
{
    [Required]
    public long ActivityId { get; set; }
}
