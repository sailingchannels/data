using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public class GenerateMainMenuResponse
    {
        public List<MainMenuSection> MainMenuSections;

        public GenerateMainMenuResponse(List<MainMenuSection> mainMenuSections)
        {
            MainMenuSections = mainMenuSections;
        }
    }
}
