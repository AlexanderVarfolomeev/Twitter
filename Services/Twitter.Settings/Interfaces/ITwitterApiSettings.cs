namespace Twitter.Settings.Interfaces;

public interface ITwitterApiSettings
{
    IDbSettings Db { get; }
    IDuendeSettings Duende { get; }
}