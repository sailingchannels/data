using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using Microsoft.Extensions.Caching.Memory;

namespace Core.UseCases
{
    public class TopicDetailUseCase : ITopicDetailUseCase
    {
        private IVideoRepository _videoRepository;
        private IChannelRepository _channelRepository;
        private ITopicRepository _topicRepository;
        private ITagRepository _tagRepository;
        private IMemoryCache _cache;
        private const string _cacheKeyPrefix = "TopicDetail";

        public TopicDetailUseCase(
            IVideoRepository videoRepository,
            IChannelRepository channelRepository,
            ITopicRepository topicRepository,
            ITagRepository tagRepository,
            IMemoryCache cache
        )
        {
            _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
            _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<TopicDetailResponse> Handle(TopicDetailRequest message)
        {
            TopicDetailResponse response;

            string cacheKey = $"{_cacheKeyPrefix}.{message.TopicID}";

            // check if the value is in cache
            if (!_cache.TryGetValue(cacheKey, out response))
            {
                response = new TopicDetailResponse();

                // get the topic based on its topic id
                response.Topic = await _topicRepository.Get(message.TopicID);

                // find channels that have any of the tags of the topic
                List<string> taggedChannelIds = await _tagRepository.SearchChannels(response.Topic.SearchTerms);

                // read matching channels
                response.Channels = await _channelRepository.GetAll(
                    taggedChannelIds,
                    0,
                    taggedChannelIds.Count,
                    response.Topic.Language);

                // find videos
                response.Videos = await _videoRepository.GetByTags(response.Topic.SearchTerms, 25);

                // update cache
                _cache.Set(
                    cacheKey,
                    response,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                );
            }

            return response;
        }
    }
}
