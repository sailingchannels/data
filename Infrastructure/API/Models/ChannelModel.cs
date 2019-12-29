using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public class ChannelModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public int PublishedAt { get; set; }
        public int Subscribers { get; set; }
        public int VideoCount { get; set; }
        public int LastUploadAt { get; set; }
        public bool SubscribersHidden { get; set; }
        public string Thumbnail { get; set; }
        public List<ChannelCustomLinkModel> CustomLinks { get; set; } = new List<ChannelCustomLinkModel>();
        public List<string> Keywords { get; set; } = new List<string>();
    }
}
