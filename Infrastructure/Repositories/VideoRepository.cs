using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class VideoRepository : IVideoRepository
    {
        private readonly IMongoCollection<Video> _col;

        public VideoRepository(IMongoCollection<Video> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        /// <summary>
        /// Search videos by text query
        /// </summary>
        /// <param name="q"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<List<Video>> Search(string q, string language = "en")
        {
            // guard clause
            if (string.IsNullOrWhiteSpace(q))
            {
                return new List<Video>();
            }

            // construct the search query
            var query = new BsonDocument()
            {
                {
                    "$text", new BsonDocument()
                    {
                        { "$search", $"\"{q}\"" }
                    }
                }
            };

            return await _col
                .Find(query)
                .Limit(Constants.SEARCH_RESULTS_MAX_COUNT)
                .ToListAsync();
        }

        /// <summary>
        /// Find all videos of a specific channel sorted by published date DESC
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<Video>> GetByChannel(string channelId, int skip = 0, int take = 5)
        {
            return await _col
                .Find(v => v.ChannelID == channelId)
                .Project<Video>("{ title: 1, description: 1, publishedAt: 1, views: 1, likes: 1, dislikes: 1 }")
                .SortByDescending(v => v.PublishedAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        /// <summary>
        /// Counts the number of videos per channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<long> CountByChannel(string channelId)
        {
            return await _col.CountDocumentsAsync(v => v.ChannelID == channelId);
        }

        /// <summary>
        /// Count all videos
        /// </summary>
        /// <returns></returns>
        public async Task<long> Count()
        {
            return await _col.CountDocumentsAsync(new BsonDocument());
        }

        /// <summary>
        /// Returns the latest video of a channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<Video> GetLatest(string channelId)
        {
            return await _col
                .Find(v => v.ChannelID == channelId)
                .Project<Video>("{ _id: 1 }")
                .SortByDescending(v => v.PublishedAt)
                .Limit(1)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Return a list of videos based on tags that they have set
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<Video>> GetByTags(List<string> tags, int take = 50)
        {
            var filter = Builders<Video>.Filter.ElemMatch(x => x.Tags, x => tags.Contains(x));

            return await _col
                .Find(filter.ToBsonDocument())
                .SortByDescending(v => v.PublishedAt)
                .Limit(take)
                .ToListAsync();
        }
    }
}
