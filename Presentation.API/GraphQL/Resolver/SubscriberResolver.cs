using System;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Infrastructure.API.Models;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class SubscriberResolver
        : IResolver, ISubscriberResolver
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMapper _mapper;

        public SubscriberResolver(
            ISubscriberRepository subscriberRepository,
            IMapper mapper
        )
        {
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException("subscriberRepository");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // TOPICS OVERVIEW
            graphQlQuery.FieldAsync<ListGraphType<SubscriberType>>(
                "subscriberHistory",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" },
                    new QueryArgument<IntGraphType> { Name = "days" }
                ),
                resolve: async context =>
                {
                    var result = await _subscriberRepository.GetHistory(
                       context.GetArgument<string>("channelId"),
                       context.GetArgument("days", 7)
                    );

                    // map entity to model
                    return _mapper.Map<List<SubscriberModel>>(result);
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
