using System;
namespace Core.DTO.UseCaseRequests
{
    public class ExtractYouTubeChannelIDRequest
    {
        public string YouTubeURL;

        public ExtractYouTubeChannelIDRequest(string youtubeUrl)
        {
            YouTubeURL = youtubeUrl;
        }
    }
}
