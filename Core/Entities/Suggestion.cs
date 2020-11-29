using System;

namespace Core.Entities
{
    public record FlagId
    {
        public string ChannelId { get; init; }
        public string UserId { get; init; }
    }

    public record Flag
    {
        public FlagId Id { get; init; }
        public DateTime When { get; init; }
    }
}
