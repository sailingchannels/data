namespace Presentation.API.GraphQL.Resolver
{
    public interface IResolver
    {
        void ResolveQuery(GraphQlQuery graphQlQuery);
        void ResolveMutation(GraphQlMutation graphQlMutation);
    }
}
