namespace Core.Entities
{
    public record VideoPublishAggregationItem
    {
        public int DayOfTheWeek { get; init; }
        public int HourOfTheDay { get; init; }
        public float PublishedVideos { get; init; }
    }
}
