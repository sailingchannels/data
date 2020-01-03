using System;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Microsoft.Extensions.Logging;
using Presentation.API.Auth;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class FlagResolver : IResolver, IFlagResolver
    {
        private readonly IFlagRepository _flagRepository;
        private readonly ILogger<FlagResolver> _logger;

        public FlagResolver(
            ILogger<FlagResolver> logger,
            IFlagRepository flagRepository
        )
        {
            _flagRepository = flagRepository ?? throw new ArgumentNullException("flagRepository");
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
                    // read user context dictionary
                    var userContext = (Dictionary<string, object>) context.UserContext;
                    string userId = Convert.ToString(userContext[ClaimTypes.UserId]);

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
            // FLAG CHANNEL
            graphQLMutation.FieldAsync<BooleanGraphType>(
                "flagChannel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" }
                ),
                resolve: async (context) =>
                {
                    // read user context dictionary
                    var userContext = (Dictionary<string, object>)context.UserContext;
                    string userId = Convert.ToString(userContext[ClaimTypes.UserId]);

                    await _flagRepository.AddFlag(
                        context.GetArgument<string>("channelId"),
                        userId
                    );

                    return true;
                });
        }
    }
}
