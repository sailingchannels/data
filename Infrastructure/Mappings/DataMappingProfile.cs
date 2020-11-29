using System;
using AutoMapper;
using Core.Entities;
using Google.Apis.YouTube.v3.Data;
using Channel = Google.Apis.YouTube.v3.Data.Channel;

namespace Infrastructure.Mappings
{
	public class DataMappingProfile : Profile
	{
		public DataMappingProfile()
		{
			CreateMap<PlaylistItem, VideoResult>()
				.ForMember(dest => dest.ID, opt => opt.MapFrom(src => src.Snippet.ResourceId.VideoId))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Snippet.Title))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Snippet.Description))
				.ForMember(dest => dest.ChannelID, opt => opt.MapFrom(src => src.Snippet.ChannelId))
				.ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src =>
                    new DateTimeOffset(src.Snippet.PublishedAt.Value).ToUnixTimeSeconds()));

			CreateMap<Channel, YouTubeChannel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Snippet.Title))
				.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Snippet.Description))
				.ForMember(dest => dest.Thumbnail, opt => opt.MapFrom(src => src.Snippet.Thumbnails.Default__.Url))
				.ForMember(dest => dest.VideoCount, opt => opt.MapFrom(src => src.Statistics.VideoCount ?? 0))
				.ForMember(dest => dest.SubscriberCount, opt => opt.MapFrom(src => src.Statistics.SubscriberCount ?? 0))
				.ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.Statistics.ViewCount ?? 0))
				.ForMember(dest => dest.HiddenSubscriberCount, opt => opt.MapFrom(src => src.Statistics.HiddenSubscriberCount ?? false));
		}
	}
}
