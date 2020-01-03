using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IYouTubeDataService
    {
        Task<List<YouTubeChannel>> GetSnippets(List<string> channelIds);
        Task<string> GetChannelIDFromUsername(string userName);
    }
}
