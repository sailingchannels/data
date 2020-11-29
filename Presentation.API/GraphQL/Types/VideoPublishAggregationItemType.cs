
using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class VideoPublishAggregationItemType
        : ObjectGraphType<VideoPublishAggregationItemModel>, IGraphQLType
    {
        public VideoPublishAggregationItemType()
        {
            Name = "VideoPublishAggregationItem";

            Field(v => v.DayOfTheWeek, nullable: false);
            Field(v => v.HourOfTheDay, nullable: false);
            Field(v => v.PublishedVideos, nullable: false);
        }
    }
}
