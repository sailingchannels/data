using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Get all suggestions by user that match the channel ids that are provided
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Suggestion>> GetAny(string userId, List<string> channelIds)
        {
            return await _col
                .Find(c => c.Id.UserId == userId && channelIds.Contains(c.Id.ChannelId))
                .ToListAsync();
        }

        /// <summary>
        /// Check if this channel has been suggested by any user before
        /// </summary>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public async Task<List<Suggestion>> GetAny(List<string> channelIds)
        {
            return await _col
                .Find(c => channelIds.Contains(c.Id.ChannelId))
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new suggestion to the list
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
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
