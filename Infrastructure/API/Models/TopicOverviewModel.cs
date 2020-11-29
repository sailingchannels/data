namespace Infrastructure.API.Models
{
    public record TopicOverviewModel
    {
        public TopicModel Topic { get; init; }
        public string LatestVideoID { get; init; }
        public string LatestChannelTitle { get; init; }
    }
}
