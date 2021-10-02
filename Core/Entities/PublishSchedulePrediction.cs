namespace Core.Entities
{
    public record PublishSchedulePrediction : Weekstamp
    {
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
