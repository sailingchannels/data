using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Entities;
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
        private const string _cacheKey = "TopicsOverview";

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
            List<TopicOverviewDTO> topicResults;

            // check if the value is in cache
            if (!_cache.TryGetValue(_cacheKey, out topicResults))
            {
                topicResults = new List<TopicOverviewDTO>();

                // read all topics
                List<Topic> topics = await _topicRepository.GetAll(message.Language);

                foreach (var topic in topics)
                {
                    // find channels that have any of the tags of the topic
                    List<string> taggedChannelIds = await _tagRepository.SearchChannels(topic.SearchTerms);

                    // find the channel with the latest upload
                    Channel latestUploadChannel = await _channelRepository.GetLastUploader(
                        taggedChannelIds,
                        Constants.TOPICS_SUBSCRIBER_THRESHOLD,
                        message.Language);

                    if (latestUploadChannel == null) continue;

                    // find the latest video of that channel
                    Video latestVideo = await _videoRepository.GetLatest(latestUploadChannel.ID);

                    topicResults.Add(
                        new TopicOverviewDTO(topic, latestVideo.ID, latestUploadChannel.Title)
                    );
                }

                // update cache
                _cache.Set(
                    _cacheKey,
                    topicResults,
                    new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                );
            }

            return new TopicsOverviewResponse(topicResults);
        }
    }
}
