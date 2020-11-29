namespace Core.Entities
{
    public class YouTubeChannel : DisplayItem
    {
        public ulong ViewCount { get; set; }
        public ulong SubscriberCount { get; set; }
        public ulong VideoCount { get; set; }
        public bool HiddenSubscriberCount { get; set; }
    }
}
