using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class VideoResolver
        : IResolver, IVideoResolver
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ChannelResolver> _logger;

        public VideoResolver(
            ILogger<ChannelResolver> logger,
            IVideoRepository videoRepository,
            IChannelRepository channelRepository,
            IMapper mapper
        )
        {
            _videoRepository = videoRepository ?? throw new ArgumentNullException("videoRepository");
            _channelRepository = channelRepository ?? throw new ArgumentNullException("channelRepository");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger;
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void ResolveQuery(GraphQLQuery graphQLQuery)
        {
            // LATEST VIDEO: returns the latest video of a channel
            graphQLQuery.FieldAsync<VideoType>(
                "latestVideo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" }
                ),
                resolve: async (context) => 
                {
                    string channelId = context.GetArgument<string>("channelId");

                    var video = await _videoRepository.GetLatest(channelId);
                    video.Channel = await _channelRepository.Get(channelId);

                    // map entity to model
                    return _mapper.Map<VideoModel>(video);
                }
            );

            // VIDEOS: returns all videos of a certain channel
            graphQLQuery.FieldAsync<ListGraphType<VideoType>>(
                "videos",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" }
                ),
                resolve: async (context) =>
                {
                    string channelId = context.GetArgument<string>("channelId");
                    var channel = await _channelRepository.Get(channelId);

                    var videos = await _videoRepository.GetByChannel(
                        channelId,
                        context.GetArgument<int>("skip"),
                        context.GetArgument<int>("take")
                    );

                    videos.ForEach(b => b.Channel = channel);

                    // map entity to model
                    return _mapper.Map<List<VideoModel>>(videos);
                }
            );

            // VIDEOS: returns all videos of a certain channel
            graphQLQuery.FieldAsync<IntGraphType>(
                "videoCount",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" }
                ),
                resolve: async (context) =>
                {
                    long videoCount = await _videoRepository.CountByChannel(
                        context.GetArgument<string>("channelId")
                    );

                    // map entity to model
                    return videoCount;
                }
            );

            // VIDEO COUNT TOTAL
            graphQLQuery.FieldAsync<IntGraphType>(
                "videoCountTotal",
                resolve: async (context) =>
                {
                    long videoCount = await _videoRepository.Count();

                    // map entity to model
                    return videoCount;
                }
            );
        }

        /// <summary>
        /// Resolves all mutations on guest accesses.
        /// </summary>
        /// <param name="graphQLMutation"></param>
        public void ResolveMutation(GraphQLMutation graphQLMutation)
        {
        }
    }
}
