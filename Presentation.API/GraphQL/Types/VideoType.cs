using Infrastructure.API.Models;
using GraphQL.Types;

namespace Presentation.API.GraphQL.Types
{
    public sealed class VideoType
        : ObjectGraphType<VideoModel>, IGraphQLType
    {
        public VideoType()
        {
            Name = "Video";

            Field(i => i.ID, nullable: false);
            Field(i => i.Title, nullable: false);
            Field(i => i.Description, nullable: false);
            Field(i => i.ChannelID, nullable: true);
            Field(i => i.PublishedAt, nullable: false);
            Field(i => i.Dislikes, nullable: false);
            Field(i => i.Likes, nullable: false);
            Field(i => i.Views, nullable: false);

            Field<ListGraphType<StringGraphType>>("Tags");
            Field<ChannelType>("Channel");
        }
    }
}
