using TimeOrganizer_net_core.exception;

namespace TimeOrganizer_net_core.helper;

public class Helper
{
    public static string GetEnvVar(string envName)
    {
        return System.Environment.GetEnvironmentVariable(envName) ?? throw new EnvironmentVariableMissingException(envName);
    }
}