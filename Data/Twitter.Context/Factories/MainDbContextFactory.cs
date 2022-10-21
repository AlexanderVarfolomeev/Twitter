using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;

namespace Twitter.Context.Factories;

public class MainDbContextFactory
{
    private readonly DbContextOptions<MainDbContext> opts;

    public MainDbContextFactory(DbContextOptions<MainDbContext> opts)
    {
        this.opts = opts;
    }

    public MainDbContext Create()
    {
        return new MainDbContext(opts);
    }
}