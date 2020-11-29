namespace Core.Entities
{
    public record ChannelPopularity
    {
        public double Total { get; init; }
        public double Subscribers { get; init; }
        public double Views { get; init; }
    }
}
