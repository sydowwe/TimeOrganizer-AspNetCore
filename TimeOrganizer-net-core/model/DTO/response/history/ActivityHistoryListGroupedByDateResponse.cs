using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.history;

public record ActivityHistoryListGroupedByDateResponse(
    DateTime date,
    List<ActivityHistoryResponse> historyResponseList
) : IResponse;