using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.history;

public record ActivityHistoryListGroupedByDateResponse(
    DateTime Date,
    List<ActivityHistoryResponse> HistoryResponseList
) : IResponse;