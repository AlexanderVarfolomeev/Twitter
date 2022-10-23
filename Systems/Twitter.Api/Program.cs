using Twitter.Api;
using Serilog;
using Twitter.Api.Configuration.ApplicationExtensions;
using Twitter.Api.Configuration.ServicesExtensions;
using Twitter.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddTwitterSerilog();

services.AddTwitterCors();

services.AddTwitterDbContext(new SettingSource());

services.AddAppServices();

services.AddEndpointsApiExplorer();

services.AddTwitterVersions();

services.AddTwitterAuth();

services.AddControllers();

services.AddTwitterSwagger();

var app = builder.Build();

app.UseTwitterSerilog();

app.UseTwitterCors();

app.UseTwitterAuth();

app.UseTwitterSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

