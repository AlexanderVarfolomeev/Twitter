using Microsoft.EntityFrameworkCore;

namespace Twitter.Context.Context;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> opts) : base(opts) { }

}