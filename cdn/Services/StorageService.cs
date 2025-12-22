namespace cdn.Services;

public interface IStorageService
{
    bool Exists(string name);
    Stream OpenRead(string name);
}

public class StorageService : IStorageService
{
    private readonly string storagePath;

    public StorageService(string storagePath)
    {
        if (string.IsNullOrWhiteSpace(storagePath))
            throw new ArgumentException("Storage path must be provided.", nameof(storagePath));

        this.storagePath = Path.GetFullPath(storagePath);
        if (!Directory.Exists(this.storagePath))
            Directory.CreateDirectory(this.storagePath);
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

    // Resolves the requested file safely inside the storage path.
    // Returns null if the path is invalid or tries to escape the storage root.
    private string? ResolvePath(string aName)
    {
        if (string.IsNullOrWhiteSpace(aName)) return null;

        // Combine and normalize
        var candidate = Path.Combine(storagePath, aName);
        try
        {
            var full = Path.GetFullPath(candidate);

            // Ensure it stays under the storage root
            var relative = Path.GetRelativePath(storagePath, full);
            if (relative.StartsWith("..") || Path.IsPathRooted(relative))
                return null;

            return full;
        }
        catch (Exception ex) when (
            ex is ArgumentException ||
            ex is NotSupportedException ||
            ex is PathTooLongException)
        {
            return null;
        }
    }
}
