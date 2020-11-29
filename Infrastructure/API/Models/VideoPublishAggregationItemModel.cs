namespace Infrastructure.API.Models
{
    public class VideoPublishAggregationItemModel
    {
        public int DayOfTheWeek { get; set; }
        public int HourOfTheDay { get; set; }
        public int PublishedVideos { get; set; }
    }
}
