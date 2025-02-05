using Codeboss.Types;

namespace ChurchManager.Domain.Features.Communications;

/// <summary>
/// Represents the communication preference of a <see cref="CommunicationType"/> in a <see cref="Person"/>.
/// </summary>
public class CommunicationType : Enumeration<CommunicationType, string>
{
    public CommunicationType(string value) => Value = value;
    
    public CommunicationType() { Value = "None"; }

    public static CommunicationType WhatsApp = new("WhatsApp");
    public static CommunicationType Signal = new("Signal");
    public static CommunicationType Email = new("Email");
    public static CommunicationType SMS = new("SMS");
    public static CommunicationType None = new("None");

    public static implicit operator CommunicationType(string value) => new(value);
}