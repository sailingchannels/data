using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISuggestionRepository
    {
        Task<IReadOnlyCollection<Suggestion>> GetAny(string userId, IReadOnlyCollection<string> channelIds);
        Task<IReadOnlyCollection<Suggestion>> GetAny(IReadOnlyCollection<string> channelIds);
        Task AddSuggestion(string channelId, string userId);
    }
}
