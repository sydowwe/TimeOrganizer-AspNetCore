namespace TimeOrganizer_net_core.exception;

public class EnvironmentVariableMissing(string name) : Exception($"Environment variable {name.ToUpper()} missing")
{
    
}