using System.Collections.Generic;

namespace Core.Entities
{
    public record MainMenuSection
    {
        public string Title { get; init; }
        public IReadOnlyCollection<MainMenuItem> Items { get; init; }
    }
}
