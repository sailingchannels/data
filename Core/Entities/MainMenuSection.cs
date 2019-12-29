using System.Collections.Generic;

namespace Core.Entities
{
    public class MainMenuSection
    {
        public string Title { get; set; }
        public List<MainMenuItem> Items { get; set; } = new List<MainMenuItem>();
    }
}
