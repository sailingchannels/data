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
                    var userContext = (GraphQLUserContext) context.UserContext;
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
                    var userContext = (GraphQLUserContext)context.UserContext;
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
