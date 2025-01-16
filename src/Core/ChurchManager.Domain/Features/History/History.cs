using System.ComponentModel.DataAnnotations;
using ChurchManager.Persistence.Shared;
using Codeboss.Types;

namespace ChurchManager.Domain.Features.History;

public class History : AuditableEntity<int>, IAggregateRoot<int>
{
    public bool IsSystem { get; set; } = false;

    [MaxLength(200)] public string Category { get; set; }

    [MaxLength(50)] public string EntityType { get; set; }

    public int EntityId { get; set; }

    [MaxLength(50)] public string Verb { get; set; }

    [MaxLength(200)] public string Caption { get; set; }

    [MaxLength(50)] public string RelatedEntityType { get; set; }

    public int? RelatedEntityId { get; set; }
    public string RelatedData { get; set; }

    [MaxLength(20)] public HistoryChangeType ChangeType { get; set; }

    [MaxLength(250)] public string ValueName { get; set; }
    public string NewValue { get; set; }
    public string NewRawValue { get; set; }
    public string OldValue { get; set; }
    public string OldRawValue { get; set; }

    public bool? IsSensitive { get; set; }

    /// <summary>
    /// Evaluates the defined value change.
    /// </summary>
    /// <param name="historyChangeList">The history change list.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="oldValue">The old defined value identifier.</param>
    /// <param name="newValue">The new defined value.</param>
    public static void EvaluateChange(HistoryChangeList historyChangeList, string propertyName, string oldValue, string newValue)
    {
        EvaluateChange(historyChangeList, propertyName, oldValue, newValue, false, string.Empty, string.Empty);
    }
    
    /// <summary>
    /// Evaluates the change.
    /// </summary>
    /// <param name="historyChangeList">The history change list.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="isSensitive">if set to <c>true</c> [is sensitive].</param>
    public static void EvaluateChange( HistoryChangeList historyChangeList, string propertyName, bool? oldValue, bool? newValue, bool isSensitive = false )
    {
        EvaluateChange(
            historyChangeList,
            propertyName,
            oldValue.HasValue ? oldValue.Value.ToString() : string.Empty,
            newValue.HasValue ? newValue.Value.ToString() : string.Empty,
            isSensitive );
    }

    /// <summary>
    /// Evaluates the change.
    /// </summary>
    /// <param name="historyChangeList">The history change list.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="isSensitive">if set to <c>true</c> [is sensitive].</param>
    /// <param name="oldRawValue">The old raw value.</param>
    /// <param name="newRawValue">The new raw value.</param>
    private static void EvaluateChange(HistoryChangeList historyChangeList, string propertyName, string oldValue,
        string newValue, bool isSensitive = false, string oldRawValue = null, string newRawValue = null)
    {
        if (!string.IsNullOrWhiteSpace(oldValue))
        {
            if (!string.IsNullOrWhiteSpace(newValue))
            {
                if (oldValue.Trim() != newValue.Trim())
                {
                    if (isSensitive)
                    {
                        historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName).SetSensitive();
                    }
                    else
                    {
                        historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName)
                            .SetNewValue(newValue).SetOldValue(oldValue).SetRawValues(oldRawValue, newRawValue);
                    }
                }
            }
            else
            {
                if (isSensitive)
                {
                    historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName).SetSensitive();
                }
                else
                {
                    historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName)
                        .SetOldValue(oldValue);
                }
            }
        }
        else if (!string.IsNullOrWhiteSpace(newValue))
        {
            if (isSensitive)
            {
                historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName).SetSensitive();
            }
            else
            {
                historyChangeList.AddChange("Modify", HistoryChangeType.Property, propertyName).SetNewValue(newValue);
            }
        }
    }
    
    /// <summary>
    /// Evaluates the change.
    /// </summary>
    /// <param name="historyChangeList">The history change list.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="includeTime">if set to <c>true</c> [include time].</param>
    /// <param name="isSensitive">if set to <c>true</c> [is sensitive].</param>
    public static void EvaluateChange( HistoryChangeList historyChangeList, string propertyName, DateTime? oldValue, DateTime? newValue, bool includeTime = false, bool isSensitive = false )
    {
        string oldStringValue = string.Empty;
        if ( oldValue.HasValue )
        {
            oldStringValue = includeTime ? oldValue.Value.ToString() : oldValue.Value.ToShortDateString();
        }

        string newStringValue = string.Empty;
        if ( newValue.HasValue )
        {
            newStringValue = includeTime ? newValue.Value.ToString() : newValue.Value.ToShortDateString();
        }

        EvaluateChange( historyChangeList, propertyName, oldStringValue, newStringValue, isSensitive );
    }
}

/// <summary>
/// Common Change Types. This get saved to History.ChangeType as a string so that custom change types can be used
/// </summary>
public class HistoryChangeType : Enumeration<HistoryChangeType, string>
{
    public HistoryChangeType(string value) => Value = value;

    /// <summary>
    /// The Change affected an entire record (for example, it was DELETED or ADDED), or is a child record of the item we are logging history for
    /// </summary>
    public static HistoryChangeType Record = new("Record");

    /// <summary>
    /// The Change affected a property on the record
    /// </summary>
    public static HistoryChangeType Property = new("Property");

    /// <summary>
    /// The Change affected an attribute value on the record
    /// </summary>
    public static HistoryChangeType Attribute = new("Attribute");

    // Implicit conversion from string
    public static implicit operator HistoryChangeType(string value) => new(value);
}