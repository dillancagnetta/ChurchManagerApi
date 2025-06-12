using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using ChurchManager.Domain.Common;
using ChurchManager.Domain.Features.ChangeRequests.Events;
using ChurchManager.Domain.Features.Churches;
using ChurchManager.Domain.Features.People;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.ChangeRequests;

/// <summary>
/// For handling external family updates to sensitive church records like baptism and Holy Spirit status
/// </summary>
[Table("ChangeRequest")]
public class ChangeRequest : Entity<int>, IAggregateRoot<int>
{
    public DateTime RequestedDate { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; }
    [MaxLength(100)] public string? Source { get; set; }
    public ApprovalStatus Status { get; set; } = ApprovalStatus.PendingApproval.Value;
    
    /// <summary>
    /// Optional church this change request is for
    /// </summary>
    public int? ChurchId { get; set; }
    
    // Review
    public int? ReviewedByPersonId { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public string? ReviewNotes { get; set; }
    
    // Navigation
    public virtual Person? ReviewedByPerson { get; set; }
    public virtual Church? Church { get; set; }
    
    public virtual ICollection<PropertyChangeRequest> Properties { get; set; } = new List<PropertyChangeRequest>();
    
    public void Approve(int reviewedByPersonId, string? notes)
    {
        ReviewedByPersonId = reviewedByPersonId;
        ReviewNotes = notes;
        ReviewedDate = DateTime.UtcNow;
        Status = ApprovalStatus.Approved;
        // Raise event
        AddDomainEvent(new ChangeRequestApprovedEvent(Id));
    }
}

[Table("PropertyChangeRequest")]
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
