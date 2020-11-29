using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IYouTubeDataService
    {
        Task<IEnumerable<YouTubeChannel>> GetChannelDetails(IEnumerable<string> channelIds, bool includingStatistics = false);
        Task<string> GetChannelIDFromUsername(string userName);
        Task<IEnumerable<VideoResult>> GetLatestVideos(string uploadsPlaylistId);
        Task<VideoStatusStatistics> GetVideoDetails(string videoId);
        Task<string> GetUploadsPlaylistId(string channelId);
    }
}
