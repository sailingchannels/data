using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class ChannelType
        : ObjectGraphType<ChannelModel>, IGraphQLType
    {
        public ChannelType()
        {
            Name = "Channel";

            Field(i => i.ID);
            Field(i => i.Title);
            Field(i => i.Description);
            Field(i => i.LastUploadAt, true);
            Field(i => i.PublishedAt);
            Field(i => i.Subscribers);
            Field(i => i.SubscribersHidden);
            Field(i => i.VideoCount);
            Field(i => i.Views);
            Field(i => i.Thumbnail, true);

            Field<ListGraphType<ChannelCustomLinkType>>("CustomLinks");
            Field<ListGraphType<StringGraphType>>("Keywords");
        }
    }
}
