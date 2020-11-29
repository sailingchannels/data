using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IYouTubeDataService
    {
        Task<IReadOnlyCollection<YouTubeChannel>> GetChannelDetails(IReadOnlyCollection<string> channelIds, bool includingStatistics = false);
        Task<string> GetChannelIdFromUsername(string userName);
        Task<IReadOnlyCollection<VideoResult>> GetLatestVideos(string uploadsPlaylistId);
        Task<VideoStatusStatistics> GetVideoDetails(string videoId);
        Task<string> GetUploadsPlaylistId(string channelId);
    }
}
