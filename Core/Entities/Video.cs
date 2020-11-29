using System.Collections.Generic;

namespace Core.Entities
{
    public record Video : DisplayItem
    {
        public ulong Views { get; init; }
        public ulong Dislikes { get; init; }
        public uint PublishedAt { get; init; }
        public ulong Likes { get; init; }
        public string ChannelId { get; init; }
        public ulong Comments { get; init; }
        public bool GeoChecked { get; init; }
        public IReadOnlyCollection<string> Tags { get; init; }
        public Channel Channel { get; init; }
    }
}
