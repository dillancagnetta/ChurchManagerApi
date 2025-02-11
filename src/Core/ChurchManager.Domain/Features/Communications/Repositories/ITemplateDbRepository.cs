using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Communications.Repositories;

public interface ITemplateDbRepository: IGenericDbRepository<CommunicationTemplate>
{
    Task<CommunicationTemplate> TemplateByNameAsync(string name, CancellationToken ct = default);
}