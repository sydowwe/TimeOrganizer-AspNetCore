using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.DTO.request.extendable;

public class NameTextRequest : IRequest
{
    [Required]
    [StringLength(50)] // Adjust length as needed
    public required string Name { get; set; }

    [StringLength(200)] // Adjust length as needed
    public string? Text { get; set; }
}
