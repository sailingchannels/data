namespace Core.Entities
{
    public record PublishSchedulePrediction
    {
        public int DayOfTheWeek { get; init; }
        public int HourOfTheDay { get; init; }
        public float PublishedVideos { get; init; }
        public float DeviationFromAverage { get; init; }

        public int DayHourIndex => DayOfTheWeek * 24 + HourOfTheDay;

        public PublishSchedulePrediction(
            VideoPublishAggregationItem item, 
            float deviationFromAverage)
        {
            DayOfTheWeek = item.DayOfTheWeek;
            HourOfTheDay = item.HourOfTheDay;
            PublishedVideos = item.PublishedVideos;
            
            DeviationFromAverage = deviationFromAverage;
        }
    }
}
