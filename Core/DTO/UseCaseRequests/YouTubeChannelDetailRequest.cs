using System.Collections.Generic;

namespace Core.DTO.UseCaseRequests
{
    public class YouTubeChannelDetailRequest
    {
        public List<string> ChannelIdsToCheck { get; private set; }

        public YouTubeChannelDetailRequest(List<string> channelIdsToCheck)
        {
            ChannelIdsToCheck = channelIdsToCheck;
        }
    }
}