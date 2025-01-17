namespace ChurchManager.Infrastructure.Abstractions.Configuration
{
    /// <summary>
    /// Represents a Application Config
    /// </summary>
    public partial class AppConfig 
    {
        public AppConfig()
        {
            SupportedCultures = new List<string>();
        }

        /// <summary>
        /// A value indicating whether to ignore ACL rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreAcl { get; set; }

        /// <summary>
        /// A value indicating whether to ignore "limit per store" rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public bool IgnoreStoreLimitations { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear /Plugins/bin directory on application startup
        /// </summary>
        public bool ClearPluginShadowDirectoryOnStartup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether copy dll plugin files to /Plugins/bin on application startup
        /// </summary>
        public bool PluginShadowCopy { get; set; }

        /// <summary>
        /// Enable the Publish/Subscribe messaging with RabbitMq to manage memory cache on every server
        /// </summary>
        public bool RabbitMqEnabled { get; set; }
        
        /// <summary>
        /// A value indicating whether to send emails 
        /// </summary>
        public bool EmailSendingEnabled { get; set; }

        /// <summary>
        /// A list of plugins to be ignored during start application - pattern
        /// </summary>
        public string PluginSkipLoadingPattern { get; set; }

        /// <summary>
        /// Enable scripting C# applications to execute code.
        /// </summary>
        public bool UseRoslynScripts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating for default cache time in minutes
        /// </summary>
        public int DefaultCacheTimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating for cookie expires in hours - default 24 * 365 = 8760
        /// </summary>
        public int CookieAuthExpires { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether we compress response
        /// </summary>
        public bool UseResponseCompression { get; set; }
        
        public IList<string> SupportedCultures { get; set; }
    }
}
