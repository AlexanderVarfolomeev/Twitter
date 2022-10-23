using Twitter.DuendeServer.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.Host.UseSerilog((host, cfg) =>
{
    cfg.ReadFrom.Configuration(host.Configuration);
});

// Add services to the container.
services.AddTwitterCors();
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddDuende();
services.AddSwaggerGen();

var app = builder.Build();

app.UseTwitterCors();

app.UseHttpsRedirection();

app.UseDuende();

app.MapControllers();

app.Run();