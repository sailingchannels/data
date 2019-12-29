using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public class SearchResultModel
    {
        public List<VideoModel> Videos { get; set; }
        public List<ChannelModel> Channels { get; set; }
    }
}
