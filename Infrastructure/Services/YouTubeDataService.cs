using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Video = Google.Apis.YouTube.v3.Data.Video;

namespace Infrastructure.Services
{
    public class YouTubeDataService : IYouTubeDataService
    {
        private YouTubeService _ytService;
        private readonly IMapper _mapper;

        public YouTubeDataService(
            YouTubeService ytService,
            IMapper mapper)
        {
            _ytService = ytService ?? throw new ArgumentNullException(nameof(YouTubeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IReadOnlyCollection<YouTubeChannel>> GetChannelDetails(
            IReadOnlyCollection<string> channelIds,
            bool includingStatistics = false
        )
        {
            if (channelIds.Count() > 50)
            {
                throw new ArgumentOutOfRangeException("channelIds must contain max. 50 entries");
            }

            string part = "snippet";

            if (includingStatistics)
            {
                part += ",statistics";
            }

            // prepare request to get channel snippets by id
            var listRequest = _ytService.Channels.List(part);
            listRequest.Id = string.Join(',', channelIds);
            listRequest.MaxResults = channelIds.Count();

            // query youtube api
            ChannelListResponse listResponse = await listRequest.ExecuteAsync();

            return _mapper.Map<List<YouTubeChannel>>(listResponse?.Items);
        }

        /// <summary>
        /// Retrieve the playlist id for channel uploads
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<string> GetUploadsPlaylistId(string channelId)
        {
            // prepare request to get channel content details by id
            var listRequest = _ytService.Channels.List("contentDetails");
            listRequest.Id = channelId;

            // query youtube api
            ChannelListResponse listResponse = await listRequest.ExecuteAsync();

            if (listResponse.Items?.Count > 0)
            {
                return listResponse.Items.First()
                    .ContentDetails.RelatedPlaylists.Uploads;
            }

            return null;
        }

        /// <summary>
        /// Return the channelid of a YouTube username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<string> GetChannelIdFromUsername(string userName)
        {
            var channelRequest = _ytService.Channels.List("id");
            channelRequest.ForUsername = userName;

            // query youtube api
            ChannelListResponse channelResponse = await channelRequest.ExecuteAsync();

            return channelResponse?.Items?.FirstOrDefault()?.Id;
        }

        /// <summary>
        /// Returns a list of video results for a channel, the latest 10 videos
        /// </summary>
        /// <param name="uploadsPlaylistId"></param>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<VideoResult>> GetLatestVideos(string uploadsPlaylistId)
        {
            // construct a request for video snippets
            var request = _ytService.PlaylistItems.List("snippet");
            request.MaxResults = 50;
            request.PlaylistId = uploadsPlaylistId;

            PlaylistItemListResponse results = await request.ExecuteAsync();

            // filter videos and take the snippet
            return _mapper.Map<List<VideoResult>>(results.Items);
        }

        /// <summary>
        /// Get statistics and status details about video
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public async Task<VideoStatusStatistics> GetVideoDetails(string videoId)
        {
            var request = _ytService.Videos.List("snippet,statistics,status");
            request.Id = videoId;

            VideoListResponse result = await request.ExecuteAsync();

            Video video = result?.Items.FirstOrDefault();

            return new VideoStatusStatistics
            {
                Comments = video.Statistics.CommentCount.GetValueOrDefault(0),
                Dislikes = video.Statistics.DislikeCount.GetValueOrDefault(0),
                Likes = video.Statistics.LikeCount.GetValueOrDefault(0),
                Views = video.Statistics.ViewCount.GetValueOrDefault(0),
                IsPrivate = video.Status.PrivacyStatus != "public",
                Tags = video.Snippet.Tags?.ToList()
            };
        }
    }
}
