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
            var menu = new List<MainMenuSection>
            {
                new()
                {
                    Title = "Channels",
                    Items = new List<MainMenuItem>
                    {
                        new("Subscribers", "/channels/subscribers"),
                        new("Views", "/channels/views"),
                        new("Last Upload", "/channels/upload"),
                        new("Founded", "/channels/founded"),
                        new("Trending", "/channels/trending")
                    }
                },
                new()
                {
                    Title = "Menu",
                    Items = new List<MainMenuItem>
                    {
                        new("Explore by topics", "/topics"),
                        new("Introduction", "/how-it-works"),
                        new("Suggest a channel", "/suggest"),
                        new("Contributions", "/contributions"),
                        new("Support Us", "/support-us")
                    }
                }
            };

            return Task.FromResult(new GenerateMainMenuResponse(menu));
        }
    }
}
