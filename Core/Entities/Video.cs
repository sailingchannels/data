using System.Collections.Generic;

namespace Core.Entities
{
	public record Video : DisplayItem
	{
		public ulong Views { get; init; }
		public uint PublishedAt { get; init; }
		public string ChannelId { get; init; }
		public Channel Channel { get; init; }
	}
}
