using System;

namespace Core.Entities
{
    public record SubscriberId
    {
        public int Date { get; init; }
        public string ChannelId { get; init; }
    }

    public record Subscriber
    {
        public SubscriberId Id { get; init; }
        public DateTime Date { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public int Year { get; init; }
        public int Subscribers { get; init; } 
    }
}
