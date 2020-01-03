using Infrastructure.API.Models;
using GraphQL.Types;

namespace Presentation.API.GraphQL.Types
{
    public sealed class ChannelType
        : ObjectGraphType<ChannelModel>, IGraphQLType
    {
        public ChannelType()
        {
            Name = "Channel";

            Field(i => i.ID, nullable: false);
            Field(i => i.Title, nullable: false);
            Field(i => i.Description, nullable: false);
            Field(i => i.LastUploadAt, nullable: true);
            Field(i => i.PublishedAt, nullable: false);
            Field(i => i.Subscribers, nullable: false);
            Field(i => i.SubscribersHidden, nullable: false);
            Field(i => i.VideoCount, nullable: false);
            Field(i => i.Views, nullable: false);
            Field(i => i.Thumbnail, nullable: true);

            Field<ListGraphType<ChannelCustomLinkType>>("CustomLinks");
            Field<ListGraphType<StringGraphType>>("Keywords");
        }
    }
}
