using System;
namespace Core.DTO.UseCaseResponses
{
    public class ExtractYouTubeChannelIDResponse
    {
        public string ChannelID;

        public ExtractYouTubeChannelIDResponse(string channelId)
        {
            ChannelID = channelId;
        }

        public ExtractYouTubeChannelIDResponse()
        {

        }
    }
}
