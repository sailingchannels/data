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

            Field(i => i.ChannelID);
            Field(i => i.Date);
            Field(i => i.Subscribers);
        }
    }
}
