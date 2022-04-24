using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoManager.Shared.Entities;

namespace TodoManager.Membership;

public class UserDbContext : IdentityDbContext<ApplicationUser, IdentityRole<long>, long>
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
    }
}
