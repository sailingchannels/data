using AutoMapper;
using Core.DTO;
using Core.DTO.UseCaseResponses;
using Core.Entities;
using Infrastructure.API.Models;

namespace Infrastructure.API
{
    public class APIModelsMappingProfile : Profile
    {
        public APIModelsMappingProfile()
        {
            CreateMap<ChannelCustomLink, ChannelCustomLinkModel>();
            CreateMap<Channel, ChannelModel>();
            CreateMap<Video, VideoModel>();
            CreateMap<Language, LanguageModel>();
            CreateMap<SearchResponse, SearchResultModel>();
            CreateMap<Topic, TopicModel>();
            CreateMap<TopicOverviewDTO, TopicOverviewModel>();
            CreateMap<TopicDetailResponse, TopicDetailModel>();
        }
    }
}
