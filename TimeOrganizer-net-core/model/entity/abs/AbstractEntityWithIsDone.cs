namespace TimeOrganizer_net_core.model.entity.abs;

public abstract class AbstractEntityWithIsDone : AbstractEntityWithActivity
{
    public bool isDone { get; set; }
}