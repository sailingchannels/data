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

        public async  Task<ExtractYouTubeChannelIDResponse> Handle(ExtractYouTubeChannelIDRequest message)
        {
            var response = new ExtractYouTubeChannelIDResponse();

            // short cut, no input data available
            if (string.IsNullOrWhiteSpace(message.YouTubeURL)) return response;

            Uri youTubeURL;
            try
            {
                youTubeURL = new Uri(message.YouTubeURL);
            }
            catch(UriFormatException)
            {
                return response;
            }

            // guard clause
            if(youTubeURL == null)
            {
                return response;
            }

            List<string> pathParts = youTubeURL.AbsolutePath.Split('/').ToList();

            // extract channelid from username via API request
            if (message.YouTubeURL.Contains("/user/"))
            {
                // extract username from youtube url
                string username = FindPartAfter(pathParts, "user");

                // extract channel id from username via youtube data api
                response.ChannelID = await _ytService.GetChannelIDFromUsername(username);
            }
            else if (message.YouTubeURL.Contains("/channel/"))
            {
                response.ChannelID = FindPartAfter(pathParts, "channel");
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
            bool next = false;

            foreach(string part in parts)
            {
                // predecessor was found, return the current part
                if(next == true)
                {
                    return part;
                }

                // the next part should be returned
                if(part == predecessor)
                {
                    next = true;
                }
            }

            return null;
        }
    }
}
