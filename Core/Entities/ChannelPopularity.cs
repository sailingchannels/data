using System;
namespace Core.Entities
{
    public class ChannelPopularity
    {
        public ChannelPopularity()
        {
        }

        public double Total { get; set; }
        public double Subscribers { get; set; }
        public double Views { get; set; }
    }
}
