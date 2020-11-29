using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public record TopicDetailModel
    {
        public TopicModel Topic { get; init; }
        public IReadOnlyCollection<VideoModel> Videos { get; init; }
        public IReadOnlyCollection<ChannelModel> Channels { get; init; }
    }
}
