namespace Infrastructure.API.Models
{
    public record PublishSchedulePredictionModel(
        int DayOfTheWeek,
        int HourOfTheDay,
        float PublishedVideos,
        float DeviationFromAverage);
}