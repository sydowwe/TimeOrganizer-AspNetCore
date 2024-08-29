using TimeOrganizer_net_core.model.DTO.request.extendable;
using TimeOrganizer_net_core.model.entity;

namespace TimeOrganizer_net_core.model.DTO.request.user;

public class LocaleRequest : IRequest
{
    public AvailableLocales Locale { get; set; }
}
