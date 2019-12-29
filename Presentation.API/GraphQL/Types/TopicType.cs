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

            Field(i => i.ID, nullable: false);
            Field(i => i.Title, nullable: false);
            Field(i => i.Description, nullable: false);
        }
    }
}
