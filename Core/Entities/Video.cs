using System.Collections.Generic;

namespace Core.Entities
{
    public class Video : DisplayItem
    {
        public int Views { get; set; }
        public int Dislikes { get; set; }
        public int PublishedAt { get; set; }
        public int Likes { get; set; }
        public string ChannelID { get; set; }
        public int Comments { get; set; }
        public bool GeoChecked { get; set; }
        public List<string> Tags { get; set; }
        public Channel Channel { get; set; }
    }
}
