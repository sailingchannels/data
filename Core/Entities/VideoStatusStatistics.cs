using System.Collections.Generic;

namespace Core.Entities
{
    public record VideoStatusStatistics
    {
        public ulong Views { get; init; }
        public ulong Likes { get; init; }
        public ulong Dislikes { get; init; }
        public ulong Comments { get; init; }
        public bool IsPrivate { get; init; }
        public IReadOnlyCollection<string> Tags { get; init; }
    }
}
