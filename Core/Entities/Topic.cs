using System.Collections.Generic;

namespace Core.Entities
{
    public record Topic
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Language { get; init; }
        public IReadOnlyCollection<string> SearchTerms { get; init; }
    }
}
