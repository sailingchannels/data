using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class VideoResolver
        : IResolver, IVideoResolver
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IAggregateVideoPublishTimesUseCase _aggregateVideoPublishTimesUseCase;
        private readonly IMapper _mapper;

        public VideoResolver(
            IVideoRepository videoRepository,
            IChannelRepository channelRepository,
            IAggregateVideoPublishTimesUseCase aggregateVideoPublishTimesUseCase,
            IMapper mapper
        )
        {
            _videoRepository = videoRepository ?? throw new ArgumentNullException("videoRepository");
            _channelRepository = channelRepository ?? throw new ArgumentNullException("channelRepository");
            _aggregateVideoPublishTimesUseCase = aggregateVideoPublishTimesUseCase ?? throw new ArgumentNullException(nameof(aggregateVideoPublishTimesUseCase));
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // LATEST VIDEO: returns the latest video of a channel
            graphQlQuery.FieldAsync<VideoType>(
                "latestVideo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" }
                ),
                resolve: async context =>
                {
                    var channelId = context.GetArgument<string>("channelId");

                    var video = await _videoRepository.GetLatest(channelId);
                    var channel = await _channelRepository.Get(channelId);

                    var enrichedVideo = video with { Channel = channel};

                    // map entity to model
                    return _mapper.Map<VideoModel>(enrichedVideo);
                }
            );

            // VIDEOS: returns all videos of a certain channel
            graphQlQuery.FieldAsync<ListGraphType<VideoType>>(
                "videos",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" }
                ),
                resolve: async context =>
                {
                    var channelId = context.GetArgument<string>("channelId");
                    var channel = await _channelRepository.Get(channelId);

                    var videos = await _videoRepository.GetByChannel(
                        channelId,
                        context.GetArgument<int>("skip"),
                        context.GetArgument<int>("take")
                    );

                    var enrichedVideos = EnrichVideosWithChannel(videos, channel);

                    // map entity to model
                    return _mapper.Map<List<VideoModel>>(enrichedVideos);
                }
            );

            // VIDEO COUNT
            graphQlQuery.FieldAsync<IntGraphType>(
                "videoCount",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" }
                ),
                resolve: async context =>
                {
                    var videoCount = await _videoRepository.CountByChannel(
                        context.GetArgument<string>("channelId")
                    );

                    return videoCount;
                }
            );

            // VIDEO COUNT TOTAL
            graphQlQuery.FieldAsync<IntGraphType>(
                "videoCountTotal",
                resolve: async context =>
                {
                    var videoCount = await _videoRepository.Count();
                    return videoCount;
                }
            );

            // VIDEO PUBLISH DISTRIBUTION
            graphQlQuery.FieldAsync<ListGraphType<VideoPublishAggregationItemType>>(
                "videoPublishDistribution",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" }
                ),
                resolve: async context =>
                {
                    try
                    {
                        var result = await _aggregateVideoPublishTimesUseCase.Handle(
                            new AggregateVideoPublishTimesRequest(
                                context.GetArgument<string>("channelId")
                            )
                        );

                        var aggregationModels = new List<VideoPublishAggregationItemModel>();

                        foreach (var (key, value) in result.Aggregation)
                        {
                            aggregationModels.AddRange(
                                value.Select(hourOfDay => new VideoPublishAggregationItemModel
                                {
                                    DayOfTheWeek = (int)key,
                                    HourOfTheDay = hourOfDay.Key,
                                    PublishedVideos = hourOfDay.Value
                                }));
                        }

                        return aggregationModels;
                    }
                    catch
                    {
                        return null;
                    }
                }
            );
        }

        /// <summary>
        /// Resolves all mutations on guest accesses.
        /// </summary>
        /// <param name="graphQlMutation"></param>
        public void ResolveMutation(GraphQlMutation graphQlMutation)
        {
        }

        private IReadOnlyCollection<Video> EnrichVideosWithChannel(
            IEnumerable<Video> videos,
            Channel channel)
        {
            var enrichedVideos = videos.Select(video =>
            {
                var enrichedVideo = video with { Channel = channel };
                return enrichedVideo;
            });

            return enrichedVideos.ToList();
        }
    }
}
