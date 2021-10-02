using System;
using Core.Interfaces.Repositories;
using GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class FlagResolver : IResolver, IFlagResolver
    {
        private readonly IFlagRepository _flagRepository;

        public FlagResolver(
            IFlagRepository flagRepository
        )
        {
            _flagRepository = flagRepository ?? throw new ArgumentNullException("flagRepository");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // LANGUAGES: a list of all lang codes
            graphQlQuery.FieldAsync<BooleanGraphType>(
                "flagExists",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" }
                ),
                resolve: async context =>
                {
                    // read user context dictionary
                    var userContext = (GraphQlUserContext) context.UserContext;
                    var userId = userContext.GetUserId();

                    // require user to be authenticated
                    if (userId == null)
                    {
                        return false;
                    }

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
        /// <param name="graphQlMutation"></param>
        public void ResolveMutation(GraphQlMutation graphQlMutation)
        {
            // FLAG CHANNEL
            graphQlMutation.FieldAsync<BooleanGraphType>(
                "flagChannel",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "channelId" }
                ),
                resolve: async context =>
                {
                    // read user context dictionary
                    var userContext = (GraphQlUserContext)context.UserContext;
                    string userId = userContext.GetUserId();

                    // require user to be authenticated
                    if (userId == null) return false;

                    await _flagRepository.AddFlag(
                        context.GetArgument<string>("channelId"),
                        userId
                    );

                    return true;
                });
        }
    }
}
