using TimeOrganizer_net_core.model.entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TimeOrganizer_net_core;

public class UserDbContext : IdentityDbContext<User>
{
    
}