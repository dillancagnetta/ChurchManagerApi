using Codeboss.Types;

namespace ChurchManager.Domain.Features.Events;

public class RegistrationStatus : Enumeration<RegistrationStatus, string>
{
    public RegistrationStatus(string value) => Value = value;

    public RegistrationStatus()
    {
        Value = "Confirmed";
    }

    public static RegistrationStatus Confirmed = new("Confirmed");
    public static RegistrationStatus Waitlisted = new("Waitlisted");
    public static RegistrationStatus Cancelled = new("Cancelled");
    
    public static implicit operator RegistrationStatus(string value) => new(value);

}