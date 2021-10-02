namespace Core.Entities
{
    public record Weekstamp
    {
        public int DayOfTheWeek { get; init; }
        public int HourOfTheDay { get; init; }
    }
}