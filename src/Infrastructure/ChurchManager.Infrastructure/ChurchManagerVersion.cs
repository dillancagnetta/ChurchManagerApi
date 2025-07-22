using System.Reflection;
using System.Text.RegularExpressions;

namespace ChurchManager.Infrastructure
{
    public static class ChurchManagerVersion
    {

        /// <summary>
        ///     Gets the major version
        /// </summary>
        public static readonly string MajorVersion = Assembly.GetExecutingAssembly().GetName().Version?.Major.ToString() ?? "1";

        /// <summary>
        ///     Gets the minor version
        /// </summary>
        public static readonly string MinorVersion = Assembly.GetExecutingAssembly().GetName().Version?.Minor.ToString() ?? "0";

        /// <summary>
        ///     Gets the full version
        /// </summary>
        public static readonly string FullVersion = $"{MajorVersion}.{MinorVersion}.{PatchVersion}";

        /// <summary>
        ///     Gets the Supported DB version
        /// </summary>
        public static readonly string SupportedDBVersion = $"{MajorVersion}.{MinorVersion}";

        /// <summary>
        ///     Gets the Supported plugin version
        /// </summary>
        public static readonly string SupportedPluginVersion = $"{MajorVersion}.{MinorVersion}";
        
        /// <summary>
        ///     Gets the patch version
        /// </summary>
        public static string PatchVersion {
            get {
                var assembly = Assembly.GetExecutingAssembly();
                if (assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute)) is not
                    AssemblyInformationalVersionAttribute infoVersionAttribute) return "0";
                var fullVersion = infoVersionAttribute.InformationalVersion;
                var match = Regex.Match(fullVersion, @"(\d+)\.(\d+)\.(\d+)(?:-([^\+]+))?", RegexOptions.Compiled, TimeSpan.FromSeconds(1));
                if (!match.Success) return "0";
                var patch = match.Groups[3].Value;
                var suffix = match.Groups[4].Value;
                return string.IsNullOrEmpty(suffix) ? patch : $"{patch}-{suffix}";
            }
        }

    }
}
