namespace Shared.Extensions;

public static class TypeOfFileExtensions
{
    public static string GetPath(this TypeOfFile type )
    {
       return Environment.CurrentDirectory + "\\wwwroot\\" + type.ToString();
    }
}