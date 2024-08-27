namespace TimeOrganizer_net_core.exception;

public class EnvironmentVariableMissingException(string name) : Exception($"Environment variable {name.ToUpper()} missing")
{
    
}