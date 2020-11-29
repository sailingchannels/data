using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public record Channel : DisplayItem
    {
        public int Views { get; init; }
        public int PublishedAt { get; init; }
        public DateTime? LastCrawl { get; init; }
        public int Subscribers { get; init; }
        public int VideoCount { get; init; }
        public string Language { get; init; }
        public string Country { get; init; }
        public bool DetectedLanguage { get; init; }
        public uint LastUploadAt { get; init; }
        public bool SubscribersHidden { get; init; }
        public ChannelPopularity Popularity { get; init; }
        public IReadOnlyCollection<string> Keywords { get; init; }
        public IReadOnlyCollection<ChannelCustomLink> CustomLinks { get; init; }
        public string UploadsPlaylistId { get; init; }
    }
}
