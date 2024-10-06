using System.Reflection;

namespace Website.Services;

public class ResourceReader
{
    public static async Task<string> GetAsync(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourcePath = assembly.GetManifestResourceNames().Single(x => x.EndsWith(name));

        using var stream = assembly.GetManifestResourceStream(resourcePath)!;
        using var reader = new StreamReader(stream);

        return await reader.ReadToEndAsync();
    }
}
