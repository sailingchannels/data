using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.Services;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class ExtractYouTubeChannelIDUseCase
        : IExtractYouTubeChannelIDUseCase
    {
        private IYouTubeDataService _ytService;

        public ExtractYouTubeChannelIDUseCase(
            IYouTubeDataService ytService
        )
        {
            _ytService = ytService ?? throw new ArgumentNullException(nameof(ytService));
        }

        public async  Task<ExtractYouTubeChannelIdResponse> Handle(ExtractYouTubeChannelIdRequest message)
        {
            var response = new ExtractYouTubeChannelIdResponse();

            if (string.IsNullOrWhiteSpace(message.YoutubeUrl))
            {
                return response;
            }

            Uri youTubeURL;
            try
            {
                youTubeURL = new Uri(message.YoutubeUrl);
            }
            catch(UriFormatException)
            {
                return response;
            }

            if(youTubeURL == null)
            {
                return response;
            }

            var pathParts = youTubeURL.AbsolutePath.Split('/').ToList();

            if (message.YoutubeUrl.Contains("/user/"))
            {
                var username = FindPartAfter(pathParts, "user");
                var channelIdFromUsername = await _ytService.GetChannelIdFromUsername(username);
                
                return response with { ChannelId = channelIdFromUsername };
            }
            else if (message.YoutubeUrl.Contains("/channel/"))
            {
                return response with { ChannelId = FindPartAfter(pathParts, "channel") };
            }

            return response;
        }

        /// <summary>
        /// Iterate a list of strings and return the item that follows a
        /// predefined predecessor
        /// </summary>
        /// <param name="parts"></param>
        /// <param name="predecessor"></param>
        /// <returns></returns>
        private string FindPartAfter(List<string> parts, string predecessor)
        {
            var next = false;

            foreach (var part in parts)
            {
                // predecessor was found, return the current part
                if (next)
                {
                    return part;
                }

                // the next part should be returned
                if (part == predecessor)
                {
                    next = true;
                }
            }

            return null;
        }
    }
}
