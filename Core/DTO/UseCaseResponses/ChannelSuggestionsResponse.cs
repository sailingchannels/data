using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public class ChannelSuggestionsResponse
    {
        public List<ChannelIdentificationDTO> Suggestions { get; private set; }

        public ChannelSuggestionsResponse(List<ChannelIdentificationDTO> suggestions)
        {
            Suggestions = suggestions;
        }
    }
}