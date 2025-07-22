using Codeboss.Results;

namespace ChurchManager.Infrastructure.Abstractions.Finances;

public interface IExternalFinanceIntegrator
{
    string AuthUrl();
    Task<OperationResult<string>> RequestAccessTokenAsync(string code, CancellationToken ct = default);
    Task<bool> HasValidTokenAsync(CancellationToken ct = default);
    Task<IEnumerable<T>> GetContactsAsync<T>(CancellationToken ct = default);
    Task<T> CreateOrUpdateContactAsync<T>(T contact, CancellationToken ct = default);
}