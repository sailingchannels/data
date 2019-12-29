namespace Core.Entities
{
    public class MainMenuItem
    {
        public string Title { get; set; }
        public string Href { get; set; }
        public bool MatchPrefix { get; set; } = false;

        public MainMenuItem(string title, string href, bool matchPrefix = false)
        {
            Title = title;
            Href = href;
            MatchPrefix = matchPrefix;
        }
    }
}
