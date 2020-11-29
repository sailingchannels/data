namespace Infrastructure.API.Models
{
    public record TopicModel
    {
        public string ID { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
    }
}
