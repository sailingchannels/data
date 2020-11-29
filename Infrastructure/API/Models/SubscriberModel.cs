namespace Infrastructure.API.Models
{
    public record SubscriberModel
    {
        public string ChannelID { get; init; }
        public int Date { get; init; }
        public int Subscribers { get; init; }
    }
}
