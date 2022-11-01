using Microsoft.EntityFrameworkCore;
using Twitter.Context.Context;

namespace Twitter.Context.Factories;

public  class DbContextOptionsFactories
{
    public static DbContextOptions<MainDbContext> Create(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<MainDbContext>();
        Configure(connectionString).Invoke(builder);
        return builder.Options;
    }

    public static Action<DbContextOptionsBuilder> Configure(string connectionString)
    {
        return (contextOptionsBuilder) => contextOptionsBuilder.UseSqlServer(connectionString);
    }
}