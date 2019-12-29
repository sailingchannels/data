using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<List<Tag>> GetByChannel(string channelId);
        Task<List<string>> SearchChannels(List<string> tags);
    }
}
