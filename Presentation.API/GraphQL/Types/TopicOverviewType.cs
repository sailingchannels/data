using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class TopicOverviewType
        : ObjectGraphType<TopicOverviewModel>, IGraphQLType
    {
        public TopicOverviewType()
        {
            Name = "TopicOverview";

            Field(i => i.LatestChannelTitle, nullable: false);
            Field(i => i.LatestVideoID, nullable: false);
            Field<TopicType>("Topic");
        }
    }
}
