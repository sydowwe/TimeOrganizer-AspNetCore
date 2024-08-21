namespace TimeOrganizer_net_core.exception;

public class UserLockedOutException(int lockOutTime) : Exception($"Locked out for {lockOutTime} minutes")
{
    
}