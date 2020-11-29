using System;
using System.Linq;
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
                .Find(v => v.ChannelId == channelId)
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
            return await _col.CountDocumentsAsync(v => v.ChannelId == channelId);
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
                .Find(v => v.ChannelId == channelId)
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

        /// <summary>
        /// Read all published at dates of channels 
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task<List<uint>> GetPublishedDates(string channelId)
        {
            var result = await _col
                .Find(c => c.ChannelId == channelId)
                .Project<Video>("{ publishedAt: 1 }")
                .ToListAsync();

            return result.Select(v => v.PublishedAt).ToList<uint>();
        }

        /// <summary>
        /// Returns all the ids from the input parameter that do exist within the
        /// videos table
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> Exist(IEnumerable<string> ids)
        {
            var result = await _col
                .Find(v => ids.Contains(v.Id))
                .Project<Video>("{ _id: 1 }")
                .ToListAsync();

            return result.Select(v => v.Id);
        }

        /// <summary>
        /// Insert new videos into datastore
        /// </summary>
        /// <param name="videos"></param>
        /// <returns></returns>
        public async Task Insert(IEnumerable<Video> videos)
        {
            await _col.InsertManyAsync(videos);
        }
    }
}
