namespace Core.Entities
{
    public abstract record DisplayItem
    {
        public string Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Thumbnail { get; init; }
    }
}
