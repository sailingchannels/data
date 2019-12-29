using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class TopicDetailType
        : ObjectGraphType<TopicDetailModel>, IGraphQLType
    {
        public TopicDetailType()
        {
            Name = "TopicDetail";

            Field<ListGraphType<VideoType>>("Videos");
            Field<ListGraphType<ChannelType>>("Channels");
            Field<TopicType>("Topic");
        }
    }
}
