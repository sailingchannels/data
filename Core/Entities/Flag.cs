using System;

namespace Core.Entities
{
    public class FlagID
    {
        public string ChannelID { get; set; }
        public string UserID { get; set; }
    }

    public class Flag
    {
        public FlagID ID { get; set; }
        public DateTime When { get; set; }
    }
}
