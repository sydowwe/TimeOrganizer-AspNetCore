using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.DTO.request.extendable;

public class TextColorRequest : IRequest
{
    [Required]
    [StringLength(500)] // Adjust length as needed
    public string text { get; set; }

    [Required]
    [StringLength(7)] // Assuming a hex color code like #FFFFFF
    public string color { get; set; }
}
