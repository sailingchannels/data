using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class PublishSchedulePredictionType : ObjectGraphType<PublishSchedulePredictionModel>, IGraphQLType
    {
        public PublishSchedulePredictionType()
        {
            Name = "PublishSchedulePrediction";

            Field(i => i.PublishedVideos);
            Field(i => i.HourOfTheDay);
            Field(i => i.DeviationFromAverage);
            Field(i => i.DayOfTheWeek);
        }
    }
}