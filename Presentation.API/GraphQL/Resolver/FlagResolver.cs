using System;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class FlagResolver
        : IResolver, IFlagResolver
    {
        private readonly IFlagRepository _flagRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FlagResolver> _logger;

        public FlagResolver(
            ILogger<FlagResolver> logger,
            IFlagRepository flagRepository,
            IMapper mapper
        )
        {
            _flagRepository = flagRepository ?? throw new ArgumentNullException("flagRepository");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger;
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void ResolveQuery(GraphQLQuery graphQLQuery)
        {
            // LANGUAGES: a list of all lang codes
            graphQLQuery.FieldAsync<BooleanGraphType>(
                "flagExists",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" }
                ),
                resolve: async (context) =>
                {
                    string userId = "";

                    // map entity to model
                    return await _flagRepository.FlagExists(
                        context.GetArgument<string>("channelId"),
                        userId
                    );
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
