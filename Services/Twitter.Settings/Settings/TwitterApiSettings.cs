using Twitter.Settings.Interfaces;
using Twitter.Settings.Source;

namespace Twitter.Settings.Settings;

public class TwitterApiSettings : ITwitterApiSettings
{
    private readonly IDbSettings _db = null!;
    private readonly ISettingSource _source;
    private readonly IDuendeSettings _duende;

    public TwitterApiSettings(ISettingSource source)
    {
        _source = source;
    }
    
    public TwitterApiSettings(IDbSettings db, ISettingSource source, IDuendeSettings duende )
    {
        _db = db;
        _source = source;
        _duende = duende;
    }

    public IDbSettings Db => _db ?? new DbSettings(_source);
    public IDuendeSettings Duende => _duende ?? new DuendeSettings(_source);
}

