using System;

namespace Core.Entities
{
    public class SubscriberID
    {
        public int Date { get; set; }
        public string ChannelID { get; set; }
    }

    public class Subscriber
    {
        public SubscriberID ID { get; set; }
        public DateTime Date { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Year { get; set; }
        public int Subscribers { get; set; } 
    }
}
