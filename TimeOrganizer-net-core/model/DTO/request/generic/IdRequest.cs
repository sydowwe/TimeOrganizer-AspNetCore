using System.ComponentModel.DataAnnotations;
using TimeOrganizer_net_core.model.DTO.request.extendable;

namespace TimeOrganizer_net_core.model.DTO.request.generic;

public class IdRequest : IRequest
{
    [Required]
    public long Id { get; set; }
}
