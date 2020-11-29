using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public class TopicDetailModel
    {
        public TopicModel Topic { get; set; }
        public List<VideoModel> Videos { get; set; }
        public List<ChannelModel> Channels { get; set; }
    }
}
