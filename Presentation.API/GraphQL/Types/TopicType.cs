using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class TopicType
        : ObjectGraphType<TopicModel>, IGraphQLType
    {
        public TopicType()
        {
            Name = "Topic";

            Field(i => i.ID);
            Field(i => i.Title);
            Field(i => i.Description);
        }
    }
}
