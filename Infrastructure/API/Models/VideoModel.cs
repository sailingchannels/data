using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public class VideoModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Views { get; set; }
        public int Dislikes { get; set; }
        public int PublishedAt { get; set; }
        public int Likes { get; set; }
        public string ChannelID { get; set; }
        public int Comments { get; set; }
        public bool GeoChecked { get; set; }
        public List<string> Tags { get; set; }
        public ChannelModel Channel { get; set; }
    }
}
