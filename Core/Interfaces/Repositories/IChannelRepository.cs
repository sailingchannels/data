using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repositories
{
    public interface IChannelRepository
    {
        Task<List<Channel>> GetAll(ChannelSorting sortBy, int skip, int take, string channelLanguage);
        Task<List<Channel>> GetAll(List<string> channelIds, int skip, int take, string channelLanguage);
        Task<Channel> Get(string id);
        Task<Channel> GetLastUploader(List<string> channelIds, int subscriberMinValue, string language);
        Task<List<Channel>> Search(string q, string language = "en");
        Task<List<Channel>> GetAll(List<string> channelIds);
        Task<long> Count();
    }
}
