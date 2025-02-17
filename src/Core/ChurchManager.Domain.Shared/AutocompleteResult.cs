﻿namespace ChurchManager.Domain.Shared
{
    public record AutocompleteResult(int? Id, string Label) { }

    public class AutocompleteResults : List<AutocompleteResult>
    {
        public AutocompleteResults(IEnumerable<AutocompleteResult> collection): base(collection)
        {
        }
    }

    public record PeopleAutocompleteViewModel(int? Id, string Label, string PhotoUrl, string ConnectionStatus) : AutocompleteResult(Id, Label);
    public class PeopleAutocompleteResults : List<PeopleAutocompleteViewModel>
    {
        public PeopleAutocompleteResults(IEnumerable<PeopleAutocompleteViewModel> collection) : base(collection)
        {
        }
    }
    
    public record GroupsAutocompleteViewModel(int? Id, string Label, string GroupType) : AutocompleteResult(Id, Label);
}
