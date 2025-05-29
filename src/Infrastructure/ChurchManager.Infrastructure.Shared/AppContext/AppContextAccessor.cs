using ChurchManager.Infrastructure.Abstractions.AppContext;

namespace ChurchManager.Infrastructure.Shared.AppContext;

public class AppContextAccessor: IAppContextAccessor
{
    private static readonly AsyncLocal<IAppContext> _asyncLocalAppContext = new();
    
    public IAppContext AppContext {
        get => _asyncLocalAppContext.Value;
        set => _asyncLocalAppContext.Value = value;
    }
}