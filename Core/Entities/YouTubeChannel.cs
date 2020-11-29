namespace Core.Entities
{
    public record YouTubeChannel : DisplayItem
    {
        public ulong ViewCount { get; init; }
        public ulong SubscriberCount { get; init; }
        public ulong VideoCount { get; init; }
        public bool HiddenSubscriberCount { get; init; }
    }
}
