using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Communication.Repositories;

public interface IPushDeviceDbRepository: IGenericDbRepository<PushDevice>
{
    Task<IList<PushDevice>> PushDevicesForPersonAsync(int personId, CancellationToken ct = default);
}