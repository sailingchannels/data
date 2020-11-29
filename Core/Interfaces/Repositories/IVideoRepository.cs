using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IVideoRepository
    {
        Task<IReadOnlyCollection<Video>> Search(string q, string language = "en");
        Task<IReadOnlyCollection<Video>> GetByChannel(string channelId, int skip = 0, int take = 5);
        Task<long> CountByChannel(string channelId);
        Task<Video> GetLatest(string channelId);
        Task<IReadOnlyCollection<Video>> GetByTags(IReadOnlyCollection<string> tags, int take = 50);
        Task<long> Count();
        Task<IReadOnlyCollection<uint>> GetPublishedDates(string channelId);
        Task<IReadOnlyCollection<string>> Exist(IReadOnlyCollection<string> ids);
        Task Insert(IReadOnlyCollection<Video> videos);
    }
}
