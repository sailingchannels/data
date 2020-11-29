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
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class ChannelResolver
        : IResolver, IChannelResolver
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IIdentifySailingChannelUseCase _identifySailingChannelUseCase;
        private readonly IMapper _mapper;
        private readonly ILogger<ChannelResolver> _logger;

        public ChannelResolver(
            ILogger<ChannelResolver> logger,
            IChannelRepository channelRepository,
            IIdentifySailingChannelUseCase identifySailingChannelUseCase,
            IMapper mapper
        )
        {
            _channelRepository = channelRepository ?? throw new ArgumentNullException("channelRepository");
            _identifySailingChannelUseCase = identifySailingChannelUseCase ?? throw new ArgumentNullException("identifySailingChannelUseCase");
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
            graphQLQuery.FieldAsync<ChannelIdentificationType>(
                "identifyChannel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelUrlHint" }
                ),
                resolve: async (context) =>
                {
                    var result = await _identifySailingChannelUseCase.Handle(
                        new IdentifySailingChannelRequest(
                            context.GetArgument<string>("channelUrlHint")
                        )
                    );

                    // truncate description
                    if (result.IdentifiedChannel != null && result.IdentifiedChannel.Channel != null)
                    {
                        result.IdentifiedChannel.Channel.Description =
                            result.IdentifiedChannel.Channel.Description.Truncate(300, ellipsis: true);
                    }

                    // map entity to model
                    return _mapper.Map<ChannelIdentificationModel>(result.IdentifiedChannel);
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
