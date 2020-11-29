namespace Infrastructure.API.Models
{
    public record ChannelIdentificationModel
    {
        public string ChannelID { get; init; }
        public DisplayItemModel Channel { get; init; }
        public bool IsSailingChannel { get; init; }
        public string Status { get; init; }
        public string Source { get; init; }
    }
}
