using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class PublishPredictionProgrammingItemType 
        : ObjectGraphType<PublishPredictionProgrammingItemModel>, IGraphQLType
    {
        public PublishPredictionProgrammingItemType()
        {
            Name = "PublishPredictionProgrammingItem";

            Field(i => i.HourOfTheDay);
            Field<ListGraphType<ChannelType>>("Channels");
            Field(i => i.DayOfTheWeek);
        }
    }
}