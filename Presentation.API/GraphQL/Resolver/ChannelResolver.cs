using System;
using System.Collections.Generic;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Extensions;
using Presentation.API.Auth;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class ChannelResolver
        : IResolver, IChannelResolver
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IIdentifySailingChannelsUseCase _identifySailingChannelsUseCase;
        private readonly IMapper _mapper;
        private readonly ILogger<ChannelResolver> _logger;

        public ChannelResolver(
            ILogger<ChannelResolver> logger,
            IChannelRepository channelRepository,
            IIdentifySailingChannelsUseCase identifySailingChannelsUseCase,
            IMapper mapper
        )
        {
            _channelRepository = channelRepository ?? throw new ArgumentNullException("channelRepository");
            _identifySailingChannelsUseCase = identifySailingChannelsUseCase ?? throw new ArgumentNullException("identifySailingChannelsUseCase");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger;
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void ResolveQuery(GraphQLQuery graphQLQuery)
        {
            // GUEST ACCESSES: list of all guest access entries
            graphQLQuery.FieldAsync<ListGraphType<ChannelType>>(
                "channels",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "sortBy" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "language" }
                ),
                resolve: async (context) => 
                {
                    var channels = await _channelRepository.GetAll(
                        (ChannelSorting) Enum.Parse(typeof(ChannelSorting), context.GetArgument<string>("sortBy")),
                        context.GetArgument<int>("skip"),
                        context.GetArgument<int>("take"),
                        context.GetArgument<string>("language")
                    );

                    // truncate description
                    foreach(var channel in channels)
                    {
                        channel.Description = channel.Description.Truncate(300, ellipsis: true);
                    }

                    // map entity to model
                    return _mapper.Map<List<ChannelModel>>(channels);
                }
            );

            // CHANNEL: retrieve single channel
            graphQLQuery.FieldAsync<ChannelType>(
                "channel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async (context) => 
                {
                    var channel = await _channelRepository.Get(context.GetArgument<string>("id"));

                    // map entity to model
                    return _mapper.Map<ChannelModel>(channel);
                }
            );

            // CHANNEL COUNT TOTAL
            graphQLQuery.FieldAsync<IntGraphType>(
                "channelCountTotal",
                resolve: async (context) =>
                {
                    long channelCount = await _channelRepository.Count();

                    // map entity to model
                    return channelCount;
                }
            );

            // IDENTIFY CHANNELS: take a list of url hints and identify sailing channels from them
            graphQLQuery.FieldAsync<ListGraphType<ChannelIdentificationType>>(
                "identifyChannels",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ListGraphType<StringGraphType>>> { Name = "channelUrlHints" }
                ),
                resolve: async (context) =>
                {
                    // read user context dictionary
                    var userContext = (Dictionary<string, object>)context.UserContext;
                    string userId = Convert.ToString(userContext[ClaimTypes.UserId]);

                    var result = await _identifySailingChannelsUseCase.Handle(
                        new IdentifySailingChannelsRequest(
                            context.GetArgument<List<string>>("channelUrlHints"),
                            userId
                        )
                    );

                    // truncate description
                    foreach (var channel in result.IdentifiedChannels)
                    {
                        channel.Channel.Description = channel.Channel.Description.Truncate(300, ellipsis: true);
                    }

                    // map entity to model
                    return _mapper.Map<List<ChannelIdentificationModel>>(result.IdentifiedChannels);
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
