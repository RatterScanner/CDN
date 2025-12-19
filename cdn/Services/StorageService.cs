namespace cdn.Services;

public interface IStorageService
{
    bool Exists(string name);
    Stream OpenRead(string name);
}

public class StorageService : IStorageService
{
    private const string STORAGE_PATH = "/storage";

    public bool Exists(string name)
    {
        var full = ResolvePath(name);
        return full is not null && File.Exists(full);
    }

    public Stream OpenRead(string name)
    {
        var full = ResolvePath(name);
        if (full is null) throw new FileNotFoundException();
        return File.OpenRead(full);
    }

    // Resolve the requested filename safely to prevent path traversal.
    // Returns full path under _storagePath or null if invalid/not allowed.
    private string? ResolvePath(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return null;

        // reject any path-separator characters to keep requests as single filenames
        if (name.IndexOf(Path.DirectorySeparatorChar) >= 0 || name.IndexOf(Path.AltDirectorySeparatorChar) >= 0)
            return null;

        // Combine and normalize
        var candidate = Path.Combine(STORAGE_PATH, name);
        try
        {
            var full = Path.GetFullPath(candidate);
            var baseFull = Path.GetFullPath(STORAGE_PATH);
            if (!full.StartsWith(baseFull + Path.DirectorySeparatorChar) && !string.Equals(full, baseFull, StringComparison.OrdinalIgnoreCase))
                return null;
            return full;
        }
        catch
        {
            return null;
        }
    }
}
