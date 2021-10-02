using System;
using GraphQL.Types;

namespace Presentation.API.GraphQL
{
    public sealed class GraphQlSchema : Schema
    {
        public GraphQlSchema(Func<Type, GraphType> resolveType)
        {
            Query = (GraphQlQuery) resolveType(typeof(GraphQlQuery));
            Mutation = (GraphQlMutation)resolveType(typeof(GraphQlMutation));
        }
    }
}
