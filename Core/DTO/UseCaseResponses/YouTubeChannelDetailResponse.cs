using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public class YouTubeChannelDetailResponse
    {
        public List<ChannelIdentificationDTO> IdentifiedChannels { get; set; } = new List<ChannelIdentificationDTO>();
    }
}