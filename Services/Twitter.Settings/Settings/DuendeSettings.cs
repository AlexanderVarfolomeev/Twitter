using Twitter.Settings.Interfaces;
using Twitter.Settings.Source;

namespace Twitter.Settings.Settings;

public class DuendeSettings : IDuendeSettings
{
    private readonly ISettingSource _source;

    public DuendeSettings(ISettingSource source)
    {
        _source = source;
    }
    
    public string Url => _source.GetAsString("IdentityServer:Url");
    public string ClientId => _source.GetAsString("IdentityServer:ClientId");
    public string ClientSecret => _source.GetAsString("IdentityServer:ClientSecret");
    public bool RequireHttps => Url.ToLower().StartsWith("https://");
}