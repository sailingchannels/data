using System;

namespace Core.Entities
{
    public record SuggestionId
    {
        public string ChannelId { get; init; }
        public string UserId { get; init; }
    }

    public class Suggestion
    {
        public SuggestionId Id { get; init; }
        public DateTime When { get; init; }
    }
}
