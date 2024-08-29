using System.Text.RegularExpressions;

namespace TimeOrganizer_net_core.helper;

public partial class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value == null ? null :
            // Convert to string and insert hyphens before each uppercase letter
            // then convert the entire string to lower case
            MyRegex().Replace(value.ToString()!, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}