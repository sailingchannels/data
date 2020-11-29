using System.Collections.Generic;

namespace Core.DTO.UseCaseRequests
{
    public record YouTubeChannelDetailRequest(IReadOnlyCollection<string> ChannelIdsToCheck);
}