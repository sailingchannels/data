namespace Infrastructure.API.Models
{
    public record DisplayItemModel
    {
        public string ID { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Thumbnail { get; init; }
    }
}
