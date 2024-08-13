using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class 
    AbstractEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id { get; set; }

    [Required]
    public DateTime createdTimestamp { get; set; }

    [Required]
    public DateTime modifiedTimestamp { get; set; }

    protected AbstractEntity()
    {
        createdTimestamp = DateTime.UtcNow;
        modifiedTimestamp = DateTime.UtcNow;
    }
}
