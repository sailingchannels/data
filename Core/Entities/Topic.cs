using System.Collections.Generic;

namespace Core.Entities
{
    public class Topic
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public List<string> SearchTerms { get; set; } = new List<string>();
    }
}
