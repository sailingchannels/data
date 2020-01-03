using System;
using System.Collections.Generic;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Extensions;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class SubscriberResolver
        : IResolver, ISubscriberResolver
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SubscriberResolver> _logger;

        public SubscriberResolver(
            ILogger<SubscriberResolver> logger,
            ISubscriberRepository subscriberRepository,
            IMapper mapper
        )
        {
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException("subscriberRepository");

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
            graphQLQuery.FieldAsync<ListGraphType<SubscriberType>>(
                "subscriberHistory",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" },
                    new QueryArgument<IntGraphType> { Name = "days" }
                ),
                resolve: async (context) =>
                {
                    var result = await _subscriberRepository.GetHistory(
                       context.GetArgument<string>("channelId"),
                       context.GetArgument<int>("days", 7)
                    );

                    // map entity to model
                    return _mapper.Map<List<SubscriberModel>>(result);
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
