using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class SubscriberType
        : ObjectGraphType<SubscriberModel>, IGraphQLType
    {
        public SubscriberType()
        {
            Name = "Subscriber";

            Field(i => i.ChannelID, nullable: false);
            Field(i => i.Date, nullable: false);
            Field(i => i.Subscribers, nullable: false);
        }
    }
}
