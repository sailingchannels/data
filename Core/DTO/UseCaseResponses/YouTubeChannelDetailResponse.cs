using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public record YouTubeChannelDetailResponse(IReadOnlyCollection<ChannelIdentificationDto> IdentifiedChannels);
}