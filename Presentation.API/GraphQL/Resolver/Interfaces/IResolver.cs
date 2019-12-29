namespace Presentation.API.GraphQL.Resolver
{
    public interface IResolver
    {
        void ResolveQuery(GraphQLQuery graphQLQuery);
        void ResolveMutation(GraphQLMutation graphQLMutation);
    }
}
