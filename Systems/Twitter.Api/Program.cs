using Microsoft.EntityFrameworkCore;
using Twitter.Api;
using Twitter.Api.Configuration;
using Serilog;
using Twitter.Context.Context;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Host.UseSerilog((host, cfg) =>
{
    cfg.ReadFrom.Configuration(host.Configuration);
});

//services.AddTwitterDbContext(new SettingSource());

//services.AddAppServices();

//services.AddEndpointsApiExplorer();

services.AddTwitterVersions();

services.AddControllers();

services.AddAppSwagger();

var app = builder.Build();

app.UseAppSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

/*
 * В configuration делают 2 папки для services и app extensions
 */