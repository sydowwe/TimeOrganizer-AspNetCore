using System.ComponentModel.DataAnnotations;

namespace TimeOrganizer_net_core.model.DTO.request.extendable;

public class NameTextColorIconRequest : NameTextColorRequest
{
    // [Required]
    // [StringLength(255)] // Adjust length as needed for the icon path or name
    // public string icon { get; set; }
}
