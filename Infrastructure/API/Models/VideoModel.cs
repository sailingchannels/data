using System.Collections.Generic;

namespace Infrastructure.API.Models
{
	public record VideoModel
	{
		public string ID { get; init; }
		public string Title { get; init; }
		public string Description { get; init; }
		public int Views { get; init; }
		public int PublishedAt { get; init; }
		public string ChannelID { get; init; }
		public ChannelModel Channel { get; init; }
	}
}
