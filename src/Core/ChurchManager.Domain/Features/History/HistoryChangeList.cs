using ChurchManager.Domain.Common;

namespace ChurchManager.Domain.Features.History;

/// <summary>
/// Helper class that can be used to keep add and track of History Changes and can be passed around to the various History methods
/// </summary>
public class HistoryChangeList : List<HistoryChange>
{
    /// <summary>
    /// Adds a HistoryChange record to the list
    /// Returns the HistoryChange object so caller can set additional property values if needed
    /// </summary>
    /// <param name="historyVerb">The history verb.</param>
    /// <param name="historyChangeType">Whether this is a property change, attribute change, or something else</param>
    /// <param name="valueName">Depending on  HistoryChangeType.Property: Property Friendly Name, Attribute =&gt; Attribute.Name, etc</param>
    /// <param name="oldValue">The value of the property prior to the change (Set this when doing a DELETE or MODIFY)</param>
    /// <param name="newValue">The value of the property after the change (Set this when doing an ADD or MODIFY)</param>
    /// <returns></returns>
    public HistoryChange AddChange(HistoryVerb historyVerb, HistoryChangeType historyChangeType, string valueName,
        string oldValue, string newValue)
    {
        var historyChange = new HistoryChange(historyVerb, historyChangeType, valueName, oldValue, newValue);
        this.Add(historyChange);
        return historyChange;
    }

    /// <summary>
    /// Adds a HistoryChange record to the list.
    /// Returns the HistoryChange object so caller can set additional property values if needed
    /// </summary>
    /// <param name="historyVerb">The history verb.</param>
    /// <param name="historyChangeType">Type of the history change.</param>
    /// <param name="valueName">Name of the value.</param>
    /// <returns></returns>
    public HistoryChange AddChange(string historyVerb, HistoryChangeType historyChangeType, string valueName)
    {
        return AddChange(historyVerb, historyChangeType, valueName, null, null);
    }

    /// <summary>
    /// Adds a custom history change that doesn't use any of the common HistoryVerbs and/or HistoryChangeTypes
    /// </summary>
    /// <param name="customVerb">The custom verb.</param>
    /// <param name="customChangeType">Type of the custom change.</param>
    /// <param name="valueName">Name of the value.</param>
    public HistoryChange AddCustom(string customVerb, string customChangeType, string valueName)
    {
        var historyChange = new HistoryChange
            { Verb = customVerb, ChangeType = customChangeType, ValueName = valueName };
        this.Add(historyChange);
        return historyChange;
    }
}