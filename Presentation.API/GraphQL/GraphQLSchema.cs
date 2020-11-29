using GraphQL.Types;
using System;

namespace Presentation.API.GraphQL
{
    public sealed class GraphQlSchema : Schema
    {
        public GraphQlSchema(Func<Type, GraphType> resolveType)
            : base()
        {
            Query = (GraphQlQuery) resolveType(typeof(GraphQlQuery));
            Mutation = (GraphQlMutation)resolveType(typeof(GraphQlMutation));
        }
    }
}
