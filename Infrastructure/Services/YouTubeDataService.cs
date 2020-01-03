using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Services;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;

namespace Infrastructure.Services
{
    public class YouTubeDataService : IYouTubeDataService
    {
        private YouTubeService _ytService;

        public YouTubeDataService()
        {
            _ytService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Environment.GetEnvironmentVariable("GOOGLE_API_KEY"),
                ApplicationName = this.GetType().ToString()
            });
        }

        /// <summary>
        /// Get a dictionary of YouTube channel informations based on a list of channel ids
        /// </summary>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public async Task<List<YouTubeChannel>> GetSnippets(List<string> channelIds)
        {
            // prepare request to get channel snippets by id
            var listRequest = _ytService.Channels.List("snippet");
            listRequest.Id = string.Join(',', channelIds);

            // query youtube api
            ChannelListResponse listResponse = await listRequest.ExecuteAsync();

            var results = new List<YouTubeChannel>();

            foreach(var channel in listResponse?.Items)
            {
                results.Add(new YouTubeChannel()
                {
                    ID = channel.Id,
                    Title = channel.Snippet.Title,
                    Description = channel.Snippet.Description,
                    Thumbnail = channel.Snippet.Thumbnails.Default__.Url
                });
            }

            return results;
        }

        /// <summary>
        /// Return the channelid of a YouTube username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> GetChannelIDFromUsername(string userName)
        {
            var channelRequest = _ytService.Channels.List("id");
            channelRequest.ForUsername = userName;

            // query youtube api
            ChannelListResponse channelResponse = await channelRequest.ExecuteAsync();

            return channelResponse?.Items?.FirstOrDefault()?.Id;
        }
    }
}
