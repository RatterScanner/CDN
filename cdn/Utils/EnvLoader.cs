namespace cdn.Utils;

public static class EnvLoader
{
    // Very small .env parser: lines like KEY=VALUE, ignores comments and blanks.
    public static void Load(string filePath = ".env")
    {
        if (!File.Exists(filePath)) return;

        foreach (var raw in File.ReadAllLines(filePath))
        {
            var line = raw.Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
            var idx = line.IndexOf('=');
            if (idx <= 0) continue;
            var key = line[..idx].Trim();
            var val = line[(idx + 1)..].Trim();
            // Remove surrounding quotes
            if ((val.StartsWith('"') && val.EndsWith('"')) || (val.StartsWith('\'') && val.EndsWith('\'')))
            {
                val = val[1..^1];
            }
            Environment.SetEnvironmentVariable(key, val);
        }
    }
}
