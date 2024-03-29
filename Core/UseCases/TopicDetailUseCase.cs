﻿using System;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
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
        private const string CacheKeyPrefix = "TopicDetail";

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
            var cacheKey = $"{CacheKeyPrefix}.{message.TopicId}";

            if (_cache.TryGetValue(cacheKey, out TopicDetailResponse response))
            {
                return response;
            }

            var topic = await _topicRepository.Get(message.TopicId);
            var taggedChannelIds = await _tagRepository.SearchChannels(topic.SearchTerms);

            var channels = await _channelRepository.GetAll(
                taggedChannelIds,
                0,
                taggedChannelIds.Count,
                topic.Language);

            var videos = await _videoRepository.GetByTags(topic.SearchTerms, 25);

            response = new TopicDetailResponse(
                topic,
                videos,
                channels);

            _cache.Set(
                cacheKey,
                response,
                new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1))
            );

            return response;
        }
    }
}
