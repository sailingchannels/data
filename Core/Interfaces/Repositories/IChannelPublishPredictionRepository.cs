using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IChannelPublishPredictionRepository
    {
        Task<bool> UpdatePrediction(ChannelPublishPrediction channelPublishPrediction);
    }
}