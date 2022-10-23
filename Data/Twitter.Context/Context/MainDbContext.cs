using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Twitter.Entities.Auth;

namespace Twitter.Context.Context;

public class MainDbContext : IdentityDbContext<TwitterUser, TwitterRole, Guid>
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) { }

}