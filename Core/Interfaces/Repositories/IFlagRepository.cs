using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IFlagRepository
    {
        Task AddFlag(string channelId, string userId);
        Task<bool> FlagExists(string channelId, string userId);
    }
}
