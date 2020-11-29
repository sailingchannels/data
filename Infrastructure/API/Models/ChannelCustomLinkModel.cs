namespace Infrastructure.API.Models
{
    public record ChannelCustomLinkModel
    {
        public string URL { get; init; }
        public string Icon { get; init; }
        public string Title { get; init; }
    }
}
