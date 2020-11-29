using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ITagRepository
    {
        Task<IReadOnlyCollection<Tag>> GetByChannel(string channelId);
        Task<IReadOnlyCollection<string>> SearchChannels(IReadOnlyCollection<string> tags);
    }
}
