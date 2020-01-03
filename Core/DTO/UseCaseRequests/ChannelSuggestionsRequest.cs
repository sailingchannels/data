using System.Collections.Generic;

namespace Core.DTO.UseCaseRequests
{
    public class ChannelSuggestionsRequest
    {
        public List<string> ChannelIds { get; private set; }
        public string Source { get; private set; }
        public string UserId { get; private set; }

        public ChannelSuggestionsRequest(List<string> channelIds, string userId, string source)
        {
            ChannelIds = channelIds;
            Source = source;
            UserId = userId;
        }
    }
}