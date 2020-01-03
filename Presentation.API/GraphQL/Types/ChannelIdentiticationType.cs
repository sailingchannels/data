using Infrastructure.API.Models;
using GraphQL.Types;
using Core.Enums;

namespace Presentation.API.GraphQL.Types
{
    public sealed class ChannelIdentificationType
        : ObjectGraphType<ChannelIdentificationModel>, IGraphQLType
    {
        public ChannelIdentificationType()
        {
            Name = "ChannelIdentification";

            Field(i => i.ChannelID, nullable: true);

            Field<DisplayItemType>("Channel");

            Field(i => i.IsSailingChannel, nullable: false);
            Field(i => i.Source, nullable: true);
            Field(i => i.Status, nullable: false);
        }
    }
}
