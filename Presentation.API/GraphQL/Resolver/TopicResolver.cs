using System;
using System.Collections.Generic;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Extensions;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class TopicResolver
        : IResolver, ITopicResolver
    {
        private readonly ITopicsOverviewUseCase _topicsOverviewUseCase;
        private readonly ITopicDetailUseCase _topicDetailUseCase;
        private readonly IMapper _mapper;
        private readonly ILogger<TopicResolver> _logger;

        public TopicResolver(
            ILogger<TopicResolver> logger,
            ITopicsOverviewUseCase topicsOverviewUseCase,
            ITopicDetailUseCase topicDetailUseCase,
            IMapper mapper
        )
        {
            _topicsOverviewUseCase = topicsOverviewUseCase ?? throw new ArgumentNullException("topicsOverviewUseCase");
            _topicDetailUseCase = topicDetailUseCase ?? throw new ArgumentNullException("topicDetailUseCase");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger;
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void ResolveQuery(GraphQLQuery graphQLQuery)
        {
            // TOPICS OVERVIEW
            graphQLQuery.FieldAsync<ListGraphType<TopicOverviewType>>(
                "topicsOverview",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "language" }
                ),
                resolve: async (context) =>
                {
                    var result = await _topicsOverviewUseCase.Handle(
                        new TopicsOverviewRequest(context.GetArgument<string>("language"))
                    );

                    // map entity to model
                    return _mapper.Map<List<TopicOverviewModel>>(result.Topics);
                }
            );

            // LANGUAGES: a list of all lang codes
            graphQLQuery.FieldAsync<TopicDetailType>(
                "topicDetail",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "topicId" }
                ),
                resolve: async (context) =>
                {
                    var result = await _topicDetailUseCase.Handle(
                        new TopicDetailRequest(context.GetArgument<string>("topicId"))
                    );

                    // truncate description
                    foreach (var channel in result.Channels)
                    {
                        channel.Description = channel.Description.Truncate(300, ellipsis: true);
                    }

                    // map entity to model
                    return _mapper.Map<TopicDetailModel>(result);
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
