using Core.Entities;

namespace Core.DTO
{
    public class TopicOverviewDTO
    {
        public Topic Topic { get; set; }
        public string LatestVideoID { get; set; }
        public string LatestChannelTitle { get; set; }

        public TopicOverviewDTO(Topic topic, string latestVideoID, string latestChannelTitle)
        {
            Topic = topic;
            LatestVideoID = latestVideoID;
            LatestChannelTitle = latestChannelTitle;
        }
    }
}
