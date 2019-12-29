namespace Core.Entities
{
    public class TagID
    {
        public string ChannelID { get; set; }
        public string Tag { get; set; }
    }

    public class Tag
    {
        public TagID ID { get; set; }
        public double Popularity { get; set; }
    }
}
