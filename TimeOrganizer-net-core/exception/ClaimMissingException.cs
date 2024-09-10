namespace TimeOrganizer_net_core.exception;

public class ClaimMissingException(string missingClaimName) : InvalidOperationException($"{missingClaimName.ToUpper()} is missing from claims")
{
    
}