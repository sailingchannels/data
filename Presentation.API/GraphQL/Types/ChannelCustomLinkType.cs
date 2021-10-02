using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class ChannelCustomLinkType
        : ObjectGraphType<ChannelCustomLinkModel>, IGraphQLType
    {
        public ChannelCustomLinkType()
        {
            Name = "ChannelCustomLink";

            Field(i => i.Title, true);
            Field(i => i.Icon, true);
            Field(i => i.URL, true);
        }
    }
}
