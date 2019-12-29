using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class FlagRepository : IFlagRepository
    {
        private readonly IMongoCollection<Flag> _col;

        public FlagRepository(IMongoCollection<Flag> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        /// <summary>
        /// Store a new flag
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task AddFlag(string channelId, string userId)
        {
            var flag = new Flag()
            {
                ID = new FlagID()
                {
                    ChannelID = channelId,
                    UserID = userId
                },
                When = DateTime.UtcNow
            };

            await _col.InsertOneAsync(flag);
        }

        /// <summary>
        /// Check if a flag exists for user and channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> FlagExists(string channelId, string userId)
        {
            return (await _col
                .CountDocumentsAsync(t => t.ID.ChannelID == channelId && t.ID.UserID == userId)) > 0;
        }
    }
}
