using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class 
    AbstractEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    public DateTime CreatedTimestamp { get; set; }

    [Required]
    public DateTime ModifiedTimestamp { get; set; }

    protected AbstractEntity()
    {
        CreatedTimestamp = DateTime.UtcNow;
        ModifiedTimestamp = DateTime.UtcNow;
    }
}
