using System.Runtime.InteropServices;

namespace NoteProject.Core.Extensions;

public static class PathExtensions
{
    public static string ConvertToAppropriateDirectorySeperator(this string path)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            path = path.Replace(@"\", "/");
        else
            path = path.Replace("/", @"\");

        return path.Replace(@"\\", @"\").Replace(@"//", @"/");
    }

    public static string ToUrl(this string url)
        => url.Replace(@"\", "/");

    public static string GetContentUrl(this string url)
        => url.Replace("NoteProject.Api", "NoteProject.Host");
}
