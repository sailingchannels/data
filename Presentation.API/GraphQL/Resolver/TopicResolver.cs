using System;
using System.Collections.Generic;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class TopicResolver
        : BaseResolver, IResolver, ITopicResolver
    {
        private readonly ITopicsOverviewUseCase _topicsOverviewUseCase;
        private readonly ITopicDetailUseCase _topicDetailUseCase;
        private readonly IMapper _mapper;

        public TopicResolver(
            ITopicsOverviewUseCase topicsOverviewUseCase,
            ITopicDetailUseCase topicDetailUseCase,
            IMapper mapper
        )
        {
            _topicsOverviewUseCase = topicsOverviewUseCase ?? throw new ArgumentNullException("topicsOverviewUseCase");
            _topicDetailUseCase = topicDetailUseCase ?? throw new ArgumentNullException("topicDetailUseCase");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // TOPICS OVERVIEW
            graphQlQuery.FieldAsync<ListGraphType<TopicOverviewType>>(
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
            graphQlQuery.FieldAsync<TopicDetailType>(
                "topicDetail",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "topicId" }
                ),
                resolve: async (context) =>
                {
                    var result = await _topicDetailUseCase.Handle(
                        new TopicDetailRequest(context.GetArgument<string>("topicId"))
                    );

                    var truncatedDescriptionResult = result with {
                        Channels = TruncateChannelDescriptions(result.Channels)
                    };
                    
                    // map entity to model
                    return _mapper.Map<TopicDetailModel>(truncatedDescriptionResult);
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
    }
}
