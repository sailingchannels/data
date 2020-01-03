using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface ISuggestionRepository
    {
        Task<List<Suggestion>> GetAny(string userId, List<string> channelIds);
        Task AddSuggestion(string channelId, string userId);
    }
}
