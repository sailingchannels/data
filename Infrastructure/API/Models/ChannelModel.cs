using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public record ChannelModel
    {
        public string ID { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int Views { get; init; }
        public int PublishedAt { get; init; }
        public int Subscribers { get; init; }
        public int VideoCount { get; init; }
        public int LastUploadAt { get; init; }
        public bool SubscribersHidden { get; init; }
        public string Thumbnail { get; init; }
        
        public IReadOnlyCollection<ChannelCustomLinkModel> CustomLinks { get; init; }
        
        public IReadOnlyCollection<string> Keywords { get; init; }
    }
}
