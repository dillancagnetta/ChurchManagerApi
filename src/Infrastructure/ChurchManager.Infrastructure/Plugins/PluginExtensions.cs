using System.Text.Json;
using ChurchManager.SharedKernel.Extensions;

namespace ChurchManager.Infrastructure.Plugins;

public sealed class PluginPaths
{
    private static PluginPaths? _instance;
    private static readonly Lock Lock = new();
    public static PluginPaths Instance => _instance ?? throw new InvalidOperationException("PluginPaths has not been initialized. Call Initialize first.");
    private readonly string _pluginPath;

    public string InstalledPluginsFile => _pluginPath;

    public static void Initialize(string settingsPath)
    {
        if (_instance == null)
        {
            lock (Lock)
            {
                _instance ??= new PluginPaths(settingsPath);
            }
        }
    }

    private PluginPaths(string pluginPath)
    {
        _pluginPath = pluginPath;
    }
}

public static class PluginExtensions
{
    public static bool OnlyInstalledPlugins(Type type)
    {
        var value = true;
        var plugin = PluginManager.FindPlugin(type);
        if (plugin != null)
        {
            return plugin.Installed;
        }
        return value;
    }

    public static IList<string>? ParseInstalledPluginsFile(string filePath)
    {
        if (!File.Exists(filePath))
            return new List<string>();

        var text = File.ReadAllText(filePath);
        if (String.IsNullOrEmpty(text))
            return new List<string>();

        return JsonSerializer.Deserialize<List<string>>(text);
    }

    public static async Task SaveInstalledPluginsFile(IList<string> pluginSystemNames, string filePath)
    {
        //serialize
        string result = JsonSerializer.Serialize(pluginSystemNames, new JsonSerializerOptions { WriteIndented = true });
        //save
        await File.WriteAllTextAsync(filePath, result);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Mark plugin as installed
    /// </summary>
    /// <param name="systemName">Plugin system name</param>
    public static async Task MarkPluginAsInstalled(string systemName)
    {
        if (string.IsNullOrEmpty(systemName))
            throw new ArgumentNullException("systemName");

        var filePath = CommonPath.InstalledPluginsFilePath;
        if (!File.Exists(filePath))
        {
            // Ensure the directory exists before creating the file
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory); 
            
            using (File.Create(filePath))
            {
                //we use 'using' to close the file after it's created
            }
        }
        
        var installedPluginSystemNames = ParseInstalledPluginsFile(filePath);

        var alreadyMarkedAsInstalled = installedPluginSystemNames.FirstOrDefault(x => x.Equals(systemName, StringComparison.OrdinalIgnoreCase)) != null;
        if (!alreadyMarkedAsInstalled)
            installedPluginSystemNames.Add(systemName);

        await SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
    }

    /// <summary>
    /// Mark plugin as uninstalled
    /// </summary>
    /// <param name="systemName">Plugin system name</param>
    public static async Task MarkPluginAsUninstalled(string systemName)
    {
        if (string.IsNullOrEmpty(systemName))
            throw new ArgumentNullException(nameof(systemName));

        var filePath = CommonPath.InstalledPluginsFilePath;
        if (!File.Exists(filePath))
        {
            // Ensure the directory exists before creating the file
            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory); 
            
            using (File.Create(filePath))
            {
                //we use 'using' to close the file after it's created
            }
        }

        var installedPluginSystemNames = ParseInstalledPluginsFile(filePath);
        var alreadyMarkedAsInstalled = installedPluginSystemNames.FirstOrDefault(x => x.Equals(systemName, StringComparison.OrdinalIgnoreCase)) != null;
        if (alreadyMarkedAsInstalled)
            installedPluginSystemNames.Remove(systemName);

        await SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
    }


}