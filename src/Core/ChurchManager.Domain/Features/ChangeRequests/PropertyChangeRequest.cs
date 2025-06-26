using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using ChurchManager.Persistence.Shared;

namespace ChurchManager.Domain.Features.ChangeRequests;

[Table("PropertyChangeRequest", Schema = "ChangeRequests")]
public class PropertyChangeRequest : Entity<int>
{
    [Required] public int ChangeRequestId { get; set; }
    // Generic entity tracking
    [Required] public string EntityType { get; set; } // "Person", "Family", "Church", etc.
    [Required] public int EntityId { get; set; }
    [Required] public string PropertyPath { get; set; } // e.g., "BaptismStatus.IsBaptised", "ReceivedHolySpirit", "FullName.FirstName"
    public string? CurrentValue { get; set; } // JSON serialized current value
    public string? RequestedValue { get; set; } // JSON serialized requested value
    public string? PropertyType { get; set; } // For deserialization hints

    public bool IsApplied { get; set; } = false;
    
    // Navigation
    public virtual ChangeRequest? ChangeRequest { get; set; }
    
    #region Helper Methods

    public void SetPropertyType(Type type) => PropertyType = type.AssemblyQualifiedName;

    public void SetCurrentValue<T>(T? value, Type? type = null)
    {
        CurrentValue = value == null ? null : JsonSerializer.Serialize(value);
        PropertyType ??= type == null ? typeof(T).AssemblyQualifiedName : type.AssemblyQualifiedName;
    }
    
    public void SetRequestedValue<T>(T? value, Type? type = null)
    {
        RequestedValue = value == null ? null : JsonSerializer.Serialize(value);
        PropertyType ??= type == null ? typeof(T).AssemblyQualifiedName : type.AssemblyQualifiedName;
    }
    
    public bool HasActualChange()
    {
        if (string.IsNullOrEmpty(CurrentValue) && string.IsNullOrEmpty(RequestedValue))
            return false;
            
        return CurrentValue != RequestedValue;
    }
    
    public string GetDisplayName()
    {
        // Convert PropertyPath to user-friendly name
        return PropertyPath switch
        {
            "BaptismStatus.IsBaptised" => "Baptism Status",
            "BaptismStatus.BaptismDate" => "Baptism Date", 
            "ReceivedHolySpirit" => "Received Holy Spirit",
            "FullName.FirstName" => "First Name",
            "FullName.LastName" => "Last Name",
            "FullName.NickName" => "Nickname",
            "Email.Address" => "Email Address",
            _ => PropertyPath.Replace(".", " - ")
        };
    }
    
    #endregion
}