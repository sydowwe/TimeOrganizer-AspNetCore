namespace TimeOrganizer_net_core.model.DTO.request.history;

public class ActivityHistoryFilterRequest : ActivitySelectForm
{
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public long? HoursBack { get; set; }
}

