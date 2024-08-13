using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.DTO.request.extendable;

public class NameTextColorRequest : NameTextRequest
{
    [Required]
    [StringLength(7)] // Assuming a hex color code like #FFFFFF
    public string color { get; set; }
}
