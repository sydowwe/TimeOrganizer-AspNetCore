using TimeOrganizer_net_core.exception;

namespace TimeOrganizer_net_core.helper;

public static class Helper
{
    public static string GetEnvVar(string envName)
    {
        return Environment.GetEnvironmentVariable(envName) ?? throw new EnvironmentVariableMissingException(envName);
    }
}