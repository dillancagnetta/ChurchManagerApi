using ChurchManager.Domain.Parameters;

namespace ChurchManager.Domain.Features.People.Queries
{
    public record PeopleAdvancedSearchQuery : SearchTermQueryParameter
    {
        public IList<string> ConnectionStatus { get; set; } = [];
        public IList<string> AgeClassification { get; set; } = [];
        public IList<string> Gender { get; set; } =[];
        public IList<string> RecordStatus { get; set; } = [];
        public IList<string> Filters { get; set; } = [];

        public int? ChurchId { get; set; }
    }
}
