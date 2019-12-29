namespace Infrastructure.API.Models
{
    public class TopicOverviewModel
    {
        public TopicModel Topic { get; set; }
        public string LatestVideoID { get; set; }
        public string LatestChannelTitle { get; set; }
    }
}
