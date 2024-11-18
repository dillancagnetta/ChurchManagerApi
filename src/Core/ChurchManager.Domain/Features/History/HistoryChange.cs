using ChurchManager.Domain.Common;
using ChurchManager.Domain.Common.Extensions;
using CodeBoss.Extensions;

namespace ChurchManager.Domain.Features.History;

/// <summary>
///
/// </summary>
public class HistoryChange
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryChange"/> class.
    /// </summary>
    public HistoryChange()
    {
        //
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryChange"/> class.
    /// </summary>
    /// <param name="historyVerb">The history verb.</param>
    /// <param name="historyChangeType">Type of the history change.</param>
    /// <param name="valueName">Name of the value.</param>
    public HistoryChange(string historyVerb, HistoryChangeType historyChangeType, string valueName)
        : this(historyVerb, historyChangeType, valueName, null, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryChange"/> class.
    /// </summary>
    /// <param name="historyVerb">The history verb.</param>
    /// <param name="historyChangeType">Whether this is a property change, attribute change, or something else</param>
    /// <param name="valueName">Depending on  HistoryChangeType.Property: Property Friendly Name, Attribute => Attribute.Name, etc </param>
    /// <param name="newValue">The new value.</param>
    /// <param name="oldValue">The old value.</param>
    public HistoryChange(HistoryVerb historyVerb, HistoryChangeType historyChangeType, string valueName, string oldValue,
        string newValue)
    {
        this.Verb = historyVerb.Value.ToUpper();
        this.ChangeType = historyChangeType;
        this.ValueName = valueName;
        this.NewValue = newValue;
        this.OldValue = oldValue;
    }

    /// <summary>
    /// Gets or sets the verb.
    /// </summary>
    /// <value>
    /// The verb.
    /// </value>
    public string Verb { get; set; }

    /// <summary>
    /// Gets or sets the type of the change.
    /// </summary>
    /// <value>
    /// The type of the change.
    /// </value>
    public string ChangeType { get; set; }

    /// <summary>
    /// Gets or sets the name of the value.
    /// </summary>
    /// <value>
    /// The name of the value.
    /// </value>
    public string ValueName { get; set; }

    /// <summary>
    /// Gets or sets the new value.
    /// </summary>
    /// <value>
    /// The new value.
    /// </value>
    public string NewValue { get; set; }

    /// <summary>
    /// Creates new rawvalue.
    /// </summary>
    /// <value>
    /// The new raw value.
    /// </value>
    public string NewRawValue { get; set; }

    /// <summary>
    /// Gets or sets the old value.
    /// </summary>
    /// <value>
    /// The old value.
    /// </value>
    public string OldValue { get; set; }

    /// <summary>
    /// Gets or sets the old raw value.
    /// </summary>
    /// <value>
    /// The old raw value.
    /// </value>
    public string OldRawValue { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is sensitive.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance is sensitive; otherwise, <c>false</c>.
    /// </value>
    public bool IsSensitive { get; set; }


    /// <summary>
    /// Gets or sets the related entity type identifier.
    /// </summary>
    /// <value>
    /// The related entity type identifier.
    /// </value>
    public string RelatedEntityType { get; set; }

    /// <summary>
    /// Gets or sets the related entity identifier.
    /// </summary>
    /// <value>
    /// The related entity identifier.
    /// </value>
    public int? RelatedEntityId { get; set; }

    /// <summary>
    /// Gets or sets the related data.
    /// </summary>
    /// <value>
    /// The related data.
    /// </value>
    public string RelatedData { get; set; }

    /// <summary>
    /// Gets the caption.
    /// </summary>
    /// <value>
    /// The caption.
    /// </value>
    public string Caption { get; set; }

    /// <summary>
    /// Gets or sets the date on which the change occurred.
    /// </summary>
    /// <value>
    /// The effective date of the change.
    /// </value>
    public DateTime? ChangedDateTime { get; set; }


    /// <summary>
    /// Sets History Record as Sensitive and ensures that OldValue and NewValue are not included
    /// </summary>
    /// <returns></returns>
    public HistoryChange SetSensitive()
    {
        this.IsSensitive = true;
        this.OldValue = null;
        this.NewValue = null;
        return this;
    }

    /// <summary>
    /// Sets the related data.
    /// </summary>
    /// <param name="relatedData">The related data.</param>
    /// <param name="relatedEntityTypeId">The related entity type identifier.</param>
    /// <param name="relatedEntityId">The related entity identifier.</param>
    /// <returns></returns>
    public HistoryChange SetRelatedData(string relatedData, string relatedEntityType, int? relatedEntityId)
    {
        this.RelatedData = relatedData;
        this.RelatedEntityType = relatedEntityType;
        this.RelatedEntityId = relatedEntityId;
        return this;
    }

    /// <summary>
    /// Sets the caption.
    /// </summary>
    /// <param name="caption">The caption.</param>
    /// <returns></returns>
    public HistoryChange SetCaption(string caption)
    {
        this.Caption = caption;
        return this;
    }

    /// <summary>
    /// Sets the value of the property after the change (set this if this is an ADD or MODIFY)
    /// </summary>
    /// <param name="newValue">The new value.</param>
    /// <returns></returns>
    public HistoryChange SetNewValue(string newValue)
    {
        this.NewValue = newValue;
        return this;
    }

    /// <summary>
    /// Sets the value of the property prior to the change (set this if this is a DELETE or MODIFY)
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <returns></returns>
    public HistoryChange SetOldValue(string oldValue)
    {
        this.OldValue = oldValue;
        return this;
    }

    /// <summary>
    /// Sets the raw values.
    /// </summary>
    /// <param name="oldRawValue">The old raw value.</param>
    /// <param name="newRawvValue">The new rawv value.</param>
    /// <returns></returns>
    public HistoryChange SetRawValues(string oldRawValue, string newRawvValue)
    {
        this.OldRawValue = oldRawValue;
        this.NewRawValue = newRawvValue;
        return this;
    }

    /// <summary>
    /// Sets the date on which the change occurred.
    /// </summary>
    /// <param name="changeDate">The date on which the change occurred.</param>
    /// <returns></returns>
    public HistoryChange SetDateOfChange(DateTime changeDate)
    {
        this.ChangedDateTime = changeDate;
        return this;
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        // create a temporary history object and set it's properties so that we can get the ToString() (the formatted summary)
        History history = new History();
        this.CopyToHistory(history);
        return history.ToString();
    }

    /// <summary>
    /// Copies the HistoryChange properties to the history record
    /// </summary>
    /// <param name="history">The history.</param>
    public void CopyToHistory(History history)
    {
        if (!string.IsNullOrEmpty(this.Caption))
        {
            // if this individual change has a Caption, use that instead of the one applied to the HistoryChangeList
            history.Caption = this.Caption.Truncate(200);
        }

        // TODO: The effective date of the change should be stored in a separate field to preserve the audit trail.
        history.CreatedDate = this.ChangedDateTime ?? DateTime.UtcNow;
        history.Verb = this.Verb;
        history.ChangeType = this.ChangeType;
        history.ValueName = this.ValueName.Truncate(250);
        history.IsSensitive = this.IsSensitive;
        history.OldValue = this.OldValue;
        history.NewValue = this.NewValue;
        history.NewRawValue = this.NewRawValue;
        history.OldRawValue = this.OldRawValue;
        if (!this.RelatedEntityType.IsNullOrEmpty())
        {
            // if this individual change has a RelatedEntityTypeId, use that instead of the one applied to the HistoryChangeList
            history.RelatedEntityType = this.RelatedEntityType;
        }

        if (this.RelatedEntityId.HasValue)
        {
            // if this individual change has a RelatedEntityId, use that instead of the one applied to the HistoryChangeList
            history.RelatedEntityId = this.RelatedEntityId;
        }

        history.RelatedData = this.RelatedData;
    }
}