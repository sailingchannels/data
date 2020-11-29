using System;
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
                var topic = await _topicRepository.Get(message.TopicID);

                var taggedChannelIds = await _tagRepository.SearchChannels(response.Topic.SearchTerms);

                var channels = await _channelRepository.GetAll(
                    taggedChannelIds,
                    0,
                    taggedChannelIds.Count,
                    response.Topic.Language);

                var videos = await _videoRepository.GetByTags(response.Topic.SearchTerms, 25);
                
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
            }

            return response;
        }
    }
}
