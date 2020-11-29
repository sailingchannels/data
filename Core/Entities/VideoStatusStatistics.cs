using System.Collections.Generic;

namespace Core.Entities
{
    public class VideoStatusStatistics
    {
        public ulong Views { get; set; }
        public ulong Likes { get; set; }
        public ulong Dislikes { get; set; }
        public ulong Comments { get; set; }
        public bool IsPrivate { get; set; } = false;
        public List<string> Tags { get; set; } = new List<string>();
    }
}
