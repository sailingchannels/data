using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class SearchUseCase : ISearchUseCase
    {
        private IVideoRepository _videoRepository;
        private IChannelRepository _channelRepository;
        private ISearchRepository _searchRepository;

        public SearchUseCase(
            IVideoRepository videoRepository,
            IChannelRepository channelRepository,
            ISearchRepository searchRepository
        ) 
        {
            _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _searchRepository = searchRepository ?? throw new ArgumentNullException(nameof(searchRepository));
        }

        public async Task<SearchResponse> Handle(SearchRequest message)
        {
            // store search object
            await _searchRepository.Insert(message.Query);
            
            var videos = await _videoRepository.Search(message.Query);
            var channels = await _channelRepository.Search(message.Query);
            
            var neededChannelIds = new HashSet<string>();
            var channelLookup = channels
                .ToDictionary(k => k.Id, v => v);

            // gather list of channel ids for the video results, that are
            // not yet fetched via the channel search results. we need to
            // fetch those additional channels as well and merge them into
            // the lookup dictionary
            foreach (var video in videos)
            {
                if (!channelLookup.ContainsKey(video.ChannelId))
                {
                    neededChannelIds.Add(video.ChannelId);
                }
            }

            var additionalChannels = await _channelRepository.GetAll(neededChannelIds.ToList());

            foreach(var additionalChannel in additionalChannels)
            {
                channelLookup.TryAdd(additionalChannel.Id, additionalChannel);
            }

            var enrichedVideos = EnrichVideosWithChannel(videos, channelLookup);

            var response = new SearchResponse(enrichedVideos, channels);
            return response;
        }

        private IReadOnlyCollection<Video> EnrichVideosWithChannel(
            IEnumerable<Video> videos, 
            Dictionary<string, Channel> channelLookup)
        {
            var enrichedVideos = videos.Select(video =>
            {
                var enrichedVideo = video with { Channel = channelLookup[video.ChannelId] };
                return enrichedVideo;
            });

            return enrichedVideos.ToList();
        }
    }
}
