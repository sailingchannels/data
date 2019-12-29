using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IVideoRepository
    {
        Task<List<Video>> Search(string q, string language = "en");
        Task<List<Video>> GetByChannel(string channelId, int skip = 0, int take = 5);
        Task<long> CountByChannel(string channelId);
        Task<Video> GetLatest(string channelId);
        Task<List<Video>> GetByTags(List<string> tags, int take = 50);
        Task<long> Count();
    }
}
