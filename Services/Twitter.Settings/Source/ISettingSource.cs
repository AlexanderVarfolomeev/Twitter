namespace Twitter.Settings.Source;

public interface ISettingSource
{
    string GetConnectionString(string? source = null);
    string GetWebRootPath();
    string GetAsString(string source, string? defaultValue = null);
    bool GetAsBool(string source, bool defaultValue = false);
    int GetAsInt(string source, int defaultValue = 0);
}