namespace ChurchManager.SharedKernel.Extensions
{
    /// <summary>
    /// Represents a common helper
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// Gets or sets application default cache time minutes
        /// </summary>
        public static int CacheTimeMinutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating for cookie expires in hours
        /// </summary>
        public static int CookieAuthExpires { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore ACL rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public static bool IgnoreAcl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to ignore "limit per store" rules (side-wide). It can significantly improve performance when enabled.
        /// </summary>
        public static bool IgnoreStoreLimitations { get; set; }

    }
}