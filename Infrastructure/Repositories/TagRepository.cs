using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using Tag = Core.Entities.Tag;

namespace Infrastructure.Repositories
{
    public sealed class TagRepository : ITagRepository
    {
        private readonly IMongoCollection<Tag> _col;

        public TagRepository(IMongoCollection<Tag> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        /// <summary>
        /// Get the 15 most popular tags for a certain channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<List<Tag>> GetByChannel(string channelId)
        {
            return await _col
                .Find(t => t.Id.ChannelId == channelId && t.Popularity > 1)
                .SortByDescending(f => f.Popularity)
                .Limit(15)
                .ToListAsync();
        }

        /// <summary>
        /// Finds a list of channel ids that have been tagged with any of the
        /// given tags parameters
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<List<string>> SearchChannels(IReadOnlyCollection<string> tags)
        {
            // construct the search query
            var query = new BsonDocument()
            {
                {
                    "$text", new BsonDocument()
                    {
                        { "$search", string.Join(" ", tags) }
                    }
                },
                {
                    "value", new BsonDocument()
                    {
                        {"$gte", 15 }
                    }
                }
            };

            // fetch distinct languges from database
            return await _col
                .DistinctAsync<string>("_id.channel", query)
                .Result
                .ToListAsync();
        }
    }
}
