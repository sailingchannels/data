using System;

namespace Core.Entities
{
    public class Search
    {
        public string ID { get; set; }
        public string Query { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
