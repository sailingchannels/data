using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Enums;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Toolkit.Extensions;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class ChannelResolver
        : BaseResolver, IResolver, IChannelResolver
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IIdentifySailingChannelUseCase _identifySailingChannelUseCase;
        private readonly IChannelPublishPredictionRepository _channelPublishPredictionRepository;
        private readonly IMapper _mapper;

        public ChannelResolver(
            IChannelRepository channelRepository,
            IIdentifySailingChannelUseCase identifySailingChannelUseCase,
            IMapper mapper, 
            IChannelPublishPredictionRepository channelPublishPredictionRepository)
        {
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _identifySailingChannelUseCase = identifySailingChannelUseCase ?? throw new ArgumentNullException(nameof(identifySailingChannelUseCase));
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _channelPublishPredictionRepository = channelPublishPredictionRepository ?? throw new ArgumentNullException(nameof(channelPublishPredictionRepository));
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // GUEST ACCESSES: list of all guest access entries
            graphQlQuery.FieldAsync<ListGraphType<ChannelType>>(
                "channels",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "sortBy" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "skip" },
                    new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "take" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "language" }
                ),
                resolve: async context => 
                {
                    var channels = await _channelRepository.GetAll(
                        (ChannelSorting) Enum.Parse(typeof(ChannelSorting), context.GetArgument<string>("sortBy")),
                        context.GetArgument<int>("skip"),
                        context.GetArgument<int>("take"),
                        context.GetArgument<string>("language")
                    );

                    var channelsWithTruncatedDescription = 
                        TruncateChannelDescriptions(channels);
                    
                    // map entity to model
                    return _mapper.Map<List<ChannelModel>>(channelsWithTruncatedDescription);
                }
            );

            // CHANNEL: retrieve single channel
            graphQlQuery.FieldAsync<ChannelType>(
                "channel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }
                ),
                resolve: async context => 
                {
                    var channel = await _channelRepository.Get(context.GetArgument<string>("id"));

                    // map entity to model
                    return _mapper.Map<ChannelModel>(channel);
                }
            );

            // CHANNEL COUNT TOTAL
            graphQlQuery.FieldAsync<IntGraphType>(
                "channelCountTotal",
                resolve: async (context) =>
                {
                    var channelCount = await _channelRepository.Count();

                    // map entity to model
                    return channelCount;
                }
            );

            // IDENTIFY CHANNELS: take a list of url hints and identify sailing channels from them
            graphQlQuery.FieldAsync<ChannelIdentificationType>(
                "identifyChannel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelUrlHint" }
                ),
                resolve: async context =>
                {
                    var result = await _identifySailingChannelUseCase.Handle(
                        new IdentifySailingChannelRequest(
                            context.GetArgument<string>("channelUrlHint")
                        )
                    );

                    var identifiedChannel = result.IdentifiedChannel;
                    
                    // truncate description
                    if (result.IdentifiedChannel != null && result.IdentifiedChannel.Channel != null)
                    {
                        identifiedChannel = identifiedChannel with {
                            Channel = identifiedChannel.Channel with {
                                Description = identifiedChannel.Channel.Description.Truncate(300, true)
                            }
                        };
                    }

                    // map entity to model
                    return _mapper.Map<ChannelIdentificationModel>(identifiedChannel);
                }
            );

            // CHANNEL PUBLISH PREDICTION
            graphQlQuery.FieldAsync<ListGraphType<PublishSchedulePredictionType>>(
                "channelPublishPrediction",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "channelId" },
                    new QueryArgument<BooleanGraphType> { Name = "filterBelowAverage", DefaultValue = false}
                ),
                resolve: async (context) =>
                {
                    var channelId = context.GetArgument<string>("channelId");
                    var filterBelowAverage = context.GetArgument<bool>("filterBelowAverage");
                    
                    var predictionResult = await _channelPublishPredictionRepository.Get(channelId);

                    if (predictionResult is null or { Gradient: <= 0.7f })
                    {
                        return null;
                    }

                    var predictionItems = predictionResult.PredictionItems;
                    
                    if (filterBelowAverage)
                    {
                        predictionItems = predictionItems.Where(i => i.DeviationFromAverage > 0);
                    }
                    
                    return _mapper.Map<List<PublishSchedulePredictionModel>>(predictionItems);
                });
        }

        /// <summary>
        /// Resolves all mutations on guest accesses.
        /// </summary>
        /// <param name="graphQlMutation"></param>
        public void ResolveMutation(GraphQlMutation graphQlMutation)
        {
        }
    }
}
