using System;

namespace Core.Entities
{
    public record Search
    {
        public string Id { get; init; }
        public string Query { get; init; }
        public DateTime Timestamp { get; init; }
    }
}
