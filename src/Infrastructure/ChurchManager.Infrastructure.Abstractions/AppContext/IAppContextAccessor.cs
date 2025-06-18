namespace ChurchManager.Infrastructure.Abstractions.AppContext;

public interface IAppContextAccessor
{
    IAppContext AppContext { get; set; }
}