using System;
using System.Linq;
using System.Collections.Generic;
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

            var response = new SearchResponse();

            // search for videos
            response.Videos = await _videoRepository.Search(message.Query);
            response.Channels = await _channelRepository.Search(message.Query);

            var neededChannelIds = new HashSet<string>();
            Dictionary<string, Channel> channelLookup = response.Channels.ToDictionary(k => k.ID, v => v);

            // gather list of channel ids for the video results, that are
            // not yet fetched via the channel search results. we need to
            // fetch those additional channels as well and merge them into
            // the loopup dictionary
            foreach (var video in response.Videos)
            {
                if(!channelLookup.ContainsKey(video.ChannelID))
                {
                    neededChannelIds.Add(video.ChannelID);
                }
            }

            // fetch additionally needed channels
            var additionalChannels = await _channelRepository.GetAll(neededChannelIds.ToList());

            foreach(var additionalChannel in additionalChannels)
            {
                channelLookup.TryAdd(additionalChannel.ID, additionalChannel);
            }

            // feed channels into videos by means of lookup table
            response.Videos.ForEach(b => b.Channel = channelLookup[b.ChannelID]);

            return response;
        }
    }
}
