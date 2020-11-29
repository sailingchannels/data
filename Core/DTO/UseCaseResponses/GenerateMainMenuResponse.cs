using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public record GenerateMainMenuResponse(IReadOnlyCollection<MainMenuSection> MainMenuSections);
}
