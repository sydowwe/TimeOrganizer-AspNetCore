namespace TimeOrganizer_net_core.exception;

public class UserNotFoundException(string email)  : NotFoundException($"User with email: {email} was not found")
{
    
}