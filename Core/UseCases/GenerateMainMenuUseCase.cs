using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseResponses;
using Core.Entities;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class GenerateMainMenuUseCase : IGenerateMainMenuUseCase
    {
        /// <summary>
        /// Generate a generic structure to render the main menu from
        /// </summary>
        /// <returns></returns>
        public Task<GenerateMainMenuResponse> Handle()
        {
            var menu = new List<MainMenuSection>()
            {
                new MainMenuSection()
                {
                    Title = "Channels",
                    Items = new List<MainMenuItem>()
                    {
                        new MainMenuItem("Subscribers", "/channels/subscribers"),
                        new MainMenuItem("Views", "/channels/views"),
                        new MainMenuItem("Last Upload", "/channels/upload"),
                        new MainMenuItem("Founded", "/channels/founded"),
                        new MainMenuItem("Trending", "/channels/trending")
                    }
                },
                new MainMenuSection()
                {
                    Title = "Menu",
                    Items = new List<MainMenuItem>()
                    {
                        new MainMenuItem("Explore by topics", "/topics"),
                        new MainMenuItem("Introduction", "/how-it-works"),
                        new MainMenuItem("Suggest a channel", "/suggest"),
                        new MainMenuItem("Contributions", "/contributions"),
                        new MainMenuItem("Support Us", "/support-us")
                    }
                }
            };

            return Task.FromResult(new GenerateMainMenuResponse(menu));
        }
    }
}
