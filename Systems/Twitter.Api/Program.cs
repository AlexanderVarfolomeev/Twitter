using Twitter.Api;
using Twitter.Api.Configuration.ApplicationExtensions;
using Twitter.Api.Configuration.ServicesExtensions;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddTwitterSerilog();

services.AddTwitterCors();

services.AddTwitterDbContext(new TwitterApiSettings(new SettingSource()));

services.AddAppServices();

services.AddEndpointsApiExplorer();

services.AddTwitterVersions();

services.AddHttpContextAccessor();

services.AddTwitterAuth();

services.AddControllers().AddTwitterValidator();

services.AddTwitterSwagger();

services.AddTwitterAutomapper();

var app = builder.Build();

app.UseTwitterMiddlewares();

app.UseTwitterSerilog();

app.UseTwitterCors();

app.UseTwitterAuth();

app.UseTwitterSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();