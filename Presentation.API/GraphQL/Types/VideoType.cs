using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
	public sealed class VideoType
		: ObjectGraphType<VideoModel>, IGraphQLType
	{
		public VideoType()
		{
			Name = "Video";

			Field(i => i.ID);
			Field(i => i.Title);
			Field(i => i.Description, true);
			Field(i => i.ChannelID, true);
			Field(i => i.PublishedAt);
			Field(i => i.Views);

			Field<ChannelType>("Channel");
		}
	}
}
