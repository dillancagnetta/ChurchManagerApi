using CodeBoss.MultiTenant;

namespace ChurchManager.Infrastructure.Abstractions.AppContext;

public interface IAppContext
{
    /// <summary>
    ///     Gets or sets the current tenant
    /// </summary>
    ITenant CurrentTenant { get; }

    /// <summary>
    ///     Gets the current sub domain
    /// </summary>
    string CurrentSubdomain { get; }
}


public interface IAppContextSetter
{
    /// <summary>
    ///    Initialize the app context
    /// </summary>
    /// <returns></returns>
    Task<IAppContext> InitializeAppContext(string subdomain, string tenantName = null);
}