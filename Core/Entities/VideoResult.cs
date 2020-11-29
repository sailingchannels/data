namespace Core.Entities
{
    public record VideoResult
    {
        public string Id { get; init; }
        public string ChannelId { get; init; }
        public string ChannelTitle { get; init; }
        public string Description { get; init; }
        public uint PublishedAt { get; init; }
        public string Title { get; init; }
    }
}
