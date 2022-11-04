using Serilog;
using Twitter.DuendeServer.Configuration;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
builder.Host.UseSerilog((host, cfg) => { cfg.ReadFrom.Configuration(host.Configuration); });
var settings = new TwitterApiSettings(new SettingSource());
services.AddAppDbContext(settings.Db);
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