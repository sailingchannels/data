using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using Microsoft.Extensions.Caching.Memory;

namespace Core.UseCases
{
    public class TopicsOverviewUseCase : ITopicsOverviewUseCase
    {
        private IVideoRepository _videoRepository;
        private IChannelRepository _channelRepository;
        private ITopicRepository _topicRepository;
        private ITagRepository _tagRepository;
        private IMemoryCache _cache;
        
        private const string CacheKey = "TopicsOverview";

        public TopicsOverviewUseCase(
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
        /// Display an overview of topics, with the latest video and channel information
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<TopicsOverviewResponse> Handle(TopicsOverviewRequest message)
        {
            List<TopicOverviewDto> topicResults;

            if (!_cache.TryGetValue(CacheKey, out topicResults))
            {
                topicResults = new List<TopicOverviewDto>();

                var topics = await _topicRepository.GetAll(message.Language);

                foreach (var topic in topics)
                {
                    var taggedChannelIds = await _tagRepository.SearchChannels(topic.SearchTerms);

                    var latestUploadChannel = await _channelRepository.GetLastUploader(
                        taggedChannelIds,
                        Constants.TOPICS_SUBSCRIBER_THRESHOLD,
                        message.Language);

                    if (latestUploadChannel == null) continue;

                    var latestVideo = await _videoRepository.GetLatest(latestUploadChannel.Id);

                    topicResults.Add(
                        new TopicOverviewDto(topic, latestVideo.Id, latestUploadChannel.Title)
                    );
                }

                _cache.Set(
                    CacheKey,
                    topicResults,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                );
            }

            return new TopicsOverviewResponse(topicResults);
        }
    }
}
