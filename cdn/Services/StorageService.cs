using System;
using System.IO;

namespace cdn.Services;

public interface IStorageService
{
    bool Exists(string name);
    Stream OpenRead(string name);
}

public class StorageService : IStorageService
{
    public const string STORAGE_PATH = "/storage";

    public StorageService()
    {
        // Ensure the directory exists
        if (!Directory.Exists(STORAGE_PATH))
            Directory.CreateDirectory(STORAGE_PATH);
    }

    public bool Exists(string aName)
    {
        var full = ResolvePath(aName);
        return full != null && File.Exists(full);
    }

    public Stream OpenRead(string aName)
    {
        var full = ResolvePath(aName) ?? throw new FileNotFoundException($"File '{aName}' not found or access denied.");
        return File.OpenRead(full);
    }

    // Resolves the requested file safely inside the storage path
    private static string? ResolvePath(string aName)
    {
        if (string.IsNullOrWhiteSpace(aName)) return null;

        var candidate = Path.Combine(STORAGE_PATH, aName);
        try
        {
            var full = Path.GetFullPath(candidate);

            // Ensure it stays under the storage root
            var relative = Path.GetRelativePath(STORAGE_PATH, full);
            if (relative.StartsWith("..") || Path.IsPathRooted(relative))
                return null;

            return full;
        }
        catch (ArgumentException) { return null; }
        catch (NotSupportedException) { return null; }
        catch (PathTooLongException) { return null; }
    }
}
