using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Twitter.Settings.Source;

public class SettingSource : ISettingSource
{
    private readonly IConfiguration? _configuration;
    
    /// <summary>
    /// Конструктор либо присваивает переданнную конфигурацию, либо считывает из файла.
    /// </summary>
    /// <param name="configuration"></param>
    public SettingSource(IConfiguration? configuration = null)
    {
        if (configuration != null)
        {
            _configuration = configuration;
            return;
        }
        
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
            .AddJsonFile("appsettings.json", optional: false);
        
        bool isDevelopment = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "")
            .ToLower().Equals("development");

        if (isDevelopment)
        {
            builder.AddJsonFile("appsettings.development.json", optional: true);
        }

        _configuration = builder.AddEnvironmentVariables()
            .Build();

    }
    
    public string GetConnectionString(string? source = null)
    {
        return ApplyEnvironmentVariable(_configuration.GetConnectionString(source ?? "ConnectionString"));
    }

    public string GetAsString(string source, string? defaultValue = null)
    {
        return ApplyEnvironmentVariable(_configuration[source]) ?? defaultValue;
    }

    public bool GetAsBool(string source, bool defaultValue = false)
    {
        var val = ApplyEnvironmentVariable(_configuration[source]);
        return bool.TryParse(val, out var result) ? result : defaultValue;
    }

    public int GetAsInt(string source, int defaultValue = 0)
    {
        var val = ApplyEnvironmentVariable(_configuration[source]);
        return int.TryParse(val, out var result) ? result : defaultValue;
    }
    
    private string ApplyEnvironmentVariable(string value)
    {
        value ??= "";
        var list = Regex.Matches(value, @"{[^}]+}").OfType<Match>().Select(c => c.Value).ToList();

        foreach (var itm in list)
        {
            var v = itm.Replace("{", "").Replace("}", "");
            value = value.Replace($"{{{v}}}", _configuration[$"{v}"]);
        }

        return value;
    }
}