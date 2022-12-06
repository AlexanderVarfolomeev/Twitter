namespace Twitter.Settings.Interfaces;

public interface IDuendeSettings
{
    string Url { get; }
    string ClientId { get; }
    string ClientSecret { get; }
    bool RequireHttps { get; }
}