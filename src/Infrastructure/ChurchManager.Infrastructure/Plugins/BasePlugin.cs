namespace ChurchManager.Infrastructure.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        /// <returns></returns>
        public virtual string? ConfigurationUrl()
        {
            return null;
        }
        /// <summary>
        /// Gets or sets the plugin info
        /// </summary>
        public virtual required PluginInfo PluginInfo { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        public virtual async Task InstallAsync(CancellationToken ct = default) 
        {
            await PluginExtensions.MarkPluginAsInstalled(PluginInfo.SystemName);
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public virtual async Task UninstallAsync(CancellationToken ct = default) 
        {
            await PluginExtensions.MarkPluginAsUninstalled(PluginInfo.SystemName);
        }

    }
}
