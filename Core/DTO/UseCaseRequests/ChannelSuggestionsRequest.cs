using System.Collections.Generic;

namespace Core.DTO.UseCaseRequests
{
    public record ChannelSuggestionsRequest(IReadOnlyCollection<string> ChannelIds, string UserId, string Source);
}