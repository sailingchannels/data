using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public record SearchResultModel
    {
        public IReadOnlyCollection<VideoModel> Videos { get; init; }
        public IReadOnlyCollection<ChannelModel> Channels { get; init; }
    }
}
