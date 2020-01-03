using Core.Enums;

namespace Infrastructure.API.Models
{
    public class ChannelIdentificationModel
    {
        public string ChannelID { get; set; }
        public DisplayItemModel Channel { get; set; }
        public bool IsSailingChannel { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
    }
}
