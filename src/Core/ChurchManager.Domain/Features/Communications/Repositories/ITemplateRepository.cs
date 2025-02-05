using ChurchManager.Domain.Features.Communications;
using ChurchManager.Infrastructure.Abstractions.Persistence;

namespace ChurchManager.Domain.Features.Communication.Repositories;

public interface ITemplateRepository: IGenericDbRepository<CommunicationTemplate>
{
    Task<CommunicationTemplate> TemplateByNameAsync(string name, CancellationToken ct = default);
}