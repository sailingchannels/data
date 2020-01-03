using System.Collections.Generic;

namespace Core.DTO.UseCaseRequests
{
    public class IdentifySailingChannelsRequest
    {
        public List<string> PossibleYouTubeChannelURLs { get; private set; }
        public string UserId { get; private set; }

        public IdentifySailingChannelsRequest(List<string> possibleYouTubeChannelURLs, string userId)
        {
            PossibleYouTubeChannelURLs = possibleYouTubeChannelURLs;
            UserId = userId;
        }
    }
}
