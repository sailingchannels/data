using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repositories
{
    public interface IChannelRepository
    {
        Task<IReadOnlyCollection<Channel>> GetAll(ChannelSorting sortBy, int skip, int take, string channelLanguage);
        Task<IReadOnlyCollection<Channel>> GetAll(IReadOnlyCollection<string> channelIds, int skip, int take, string channelLanguage);
        Task<Channel> Get(string id);
        Task<Channel> GetLastUploader(IReadOnlyCollection<string> channelIds, int subscriberMinValue, string language);
        Task<IReadOnlyCollection<Channel>> Search(string q, string language = "en");
        Task<IReadOnlyCollection<Channel>> GetAll(IReadOnlyCollection<string> channelIds);
        Task<long> Count();
        Task<IReadOnlyCollection<Channel>> GetAllChannelIdAndTitle();
        Task<IReadOnlyCollection<Channel>> GetIDsByLastUploadTimerange(DateTime from, DateTime until);
        Task UpdateNewVideoWasFound(string channelId, uint lastUploadTimestamp);
        Task<uint> GetLastUploadTimestamp(string channelId);
    }
}
