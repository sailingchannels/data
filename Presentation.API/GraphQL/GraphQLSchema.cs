using GraphQL.Types;
using System;

namespace Presentation.API.GraphQL
{
    public sealed class GraphQLSchema : Schema
    {
        public GraphQLSchema(Func<Type, GraphType> resolveType)
            : base()
        {
            Query = (GraphQLQuery) resolveType(typeof(GraphQLQuery));
            Mutation = (GraphQLMutation)resolveType(typeof(GraphQLMutation));
        }
    }
}
