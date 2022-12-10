using Twitter.Api;
using Twitter.Api.Configuration.ApplicationExtensions;
using Twitter.Api.Configuration.ServicesExtensions;
using Twitter.Settings.Settings;
using Twitter.Settings.Source;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var settings = new TwitterApiSettings(new SettingSource());
builder.AddTwitterSerilog();

services.AddTwitterCors();

services.AddTwitterDbContext(settings);

services.AddAppServices();

services.AddEndpointsApiExplorer();

services.AddTwitterVersions();

services.AddHttpContextAccessor();

services.AddTwitterAuth(settings);

services.AddControllers().AddTwitterValidator();

services.AddTwitterSwagger(settings);

services.AddTwitterAutomapper();

var app = builder.Build();

app.UseTwitterMiddlewares();

app.UseTwitterSerilog();

app.UseTwitterCors();

app.UseTwitterAuth();

app.UseTwitterSwagger();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseTwitterDbContext();
app.Run();