namespace Core.Entities
{
    public record TagId
    {
        public string ChannelId { get; init; }
        public string Tag { get; init; }
    }

    public record Tag
    {
        public TagId Id { get; init; }
        public double Popularity { get; init; }
    }
}
