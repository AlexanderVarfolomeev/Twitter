using Microsoft.AspNetCore.Http;
namespace Shared.Extensions;

public static class FormFileExtensions
{
    public static bool IsImage(this IFormFile postedFile)
    {
        if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            return false;

        var postedFileExtension = Path.GetExtension(postedFile.FileName);
        if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}