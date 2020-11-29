using System.Collections.Generic;

namespace Core.Entities
{
    public class Video : DisplayItem
    {
        public ulong Views { get; set; }
        public ulong Dislikes { get; set; }
        public uint PublishedAt { get; set; }
        public ulong Likes { get; set; }
        public string ChannelID { get; set; }
        public ulong Comments { get; set; }
        public bool GeoChecked { get; set; }
        public List<string> Tags { get; set; }
        public Channel Channel { get; set; }
    }
}
