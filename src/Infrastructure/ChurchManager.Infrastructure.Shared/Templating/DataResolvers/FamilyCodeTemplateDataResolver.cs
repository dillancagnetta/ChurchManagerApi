using ChurchManager.Domain.Features.People.Repositories;
using ChurchManager.Infrastructure.Shared.Templating.DataResolvers;

namespace ChurchManager.Infrastructure.Shared.Templating;

[TemplateName("FamilyCodeRequest")]
public class FamilyCodeTemplateDataResolver(IPersonDbRepository personDb) : TemplateDataResolverBase(personDb)
{
    protected override async Task<Dictionary<string, string>> ResolveTemplateSpecificDataAsync(int personId, IDictionary<string, object> additionalData, CancellationToken ct = default)
    {
        /****  EXAMPLE ***
         
                if (!additionalData?.TryGetValue("appointmentId", out var appointmentId) ?? true)
                {
                    throw new ArgumentException("Appointment ID is required for appointment emails");
                }

                var appointment = await _appointmentService.GetAppointmentAsync(appointmentId.ToString());
         */
        
        var familyCode = await personDb.FamilyCode(personId, ct);
        
        return new Dictionary<string, string>
        {
            ["FamilyCode"] = familyCode,
        };
    }
}