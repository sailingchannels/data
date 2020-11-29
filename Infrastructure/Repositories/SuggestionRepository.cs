using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class SuggestionRepository : ISuggestionRepository
    {
        private readonly IMongoCollection<Suggestion> _col;

        public SuggestionRepository(IMongoCollection<Suggestion> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        public async Task<IReadOnlyCollection<Suggestion>> GetAny(string userId, IReadOnlyCollection<string> channelIds)
        {
            return await _col
                .Find(c => c.Id.UserId == userId && channelIds.Contains(c.Id.ChannelId))
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Suggestion>> GetAny(IReadOnlyCollection<string> channelIds)
        {
            return await _col
                .Find(c => channelIds.Contains(c.Id.ChannelId))
                .ToListAsync();
        }

        public async Task AddSuggestion(string channelId, string userId)
        {
            var flag = new Suggestion()
            {
                Id = new SuggestionId()
                {
                    ChannelId = channelId,
                    UserId = userId
                },
                When = DateTime.UtcNow
            };

            await _col.InsertOneAsync(flag);
        }
    }
}
