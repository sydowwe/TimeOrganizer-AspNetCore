using TimeOrganizer_net_core.model.DTO.response.generic;

namespace TimeOrganizer_net_core.model.DTO.response.history;

public record HistoryListGroupedByDateResponse(
    DateTime date,
    List<HistoryResponse> historyResponseList
) : IResponse;