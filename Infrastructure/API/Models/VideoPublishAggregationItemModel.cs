namespace Infrastructure.API.Models
{
    public record VideoPublishAggregationItemModel
    {
        public int DayOfTheWeek { get; init; }
        public int HourOfTheDay { get; init; }
        public int PublishedVideos { get; init; }
    }
}
