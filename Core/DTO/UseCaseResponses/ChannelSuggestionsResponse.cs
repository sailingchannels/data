using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public record ChannelSuggestionsResponse(IReadOnlyCollection<ChannelIdentificationDto> Suggestions);
}