namespace Twitter.Settings.Interfaces;

public interface IDbSettings
{
    string GetConnectionString { get; }
}