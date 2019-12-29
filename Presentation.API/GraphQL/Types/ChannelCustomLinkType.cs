using Infrastructure.API.Models;
using GraphQL.Types;

namespace Presentation.API.GraphQL.Types
{
    public sealed class ChannelCustomLinkType
        : ObjectGraphType<ChannelCustomLinkModel>, IGraphQLType
    {
        public ChannelCustomLinkType()
        {
            Name = "ChannelCustomLink";

            Field(i => i.Title, nullable: true);
            Field(i => i.Icon, nullable: true);
            Field(i => i.URL, nullable: true);
        }
    }
}
