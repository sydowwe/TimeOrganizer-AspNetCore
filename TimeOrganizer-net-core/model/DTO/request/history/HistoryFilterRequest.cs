namespace TimeOrganizer_net_core.model.DTO.request.history;

public class HistoryFilterRequest : ActivitySelectForm
{
    public DateTime? dateFrom { get; set; }
    public DateTime? dateTo { get; set; }
    public long? hoursBack { get; set; }
}

