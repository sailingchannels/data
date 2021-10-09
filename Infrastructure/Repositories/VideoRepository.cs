using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IReadOnlyCollection<Video>> Search(string q, string language = "en")
        {
            // guard clause
            if (string.IsNullOrWhiteSpace(q))
            {
                return new List<Video>();
            }

            // construct the search query
            var query = new BsonDocument
            {
                {
                    "$text", new BsonDocument
                    {
                        { "$search", q }
                    }
                }
            };

            return await _col
                .Find(query)
                .Limit(Constants.SEARCH_RESULTS_MAX_COUNT)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Video>> GetByChannel(string channelId, int skip = 0, int take = 5)
        {
            return await _col
                .Find(v => v.ChannelId == channelId)
                .Project<Video>("{ title: 1, description: 1, publishedAt: 1, views: 1, likes: 1, dislikes: 1 }")
                .SortByDescending(v => v.PublishedAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public async Task<long> CountByChannel(string channelId)
        {
            return await _col.CountDocumentsAsync(v => v.ChannelId == channelId);
        }

        public async Task<long> Count()
        {
            return await _col.CountDocumentsAsync(new BsonDocument());
        }

        public async Task<Video> GetLatest(string channelId)
        {
            return await _col
                .Find(v => v.ChannelId == channelId)
                .Project<Video>("{ _id: 1 }")
                .SortByDescending(v => v.PublishedAt)
                .Limit(1)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Video>> GetByTags(IReadOnlyCollection<string> tags, int take = 50)
        {
            var filter = Builders<Video>.Filter.ElemMatch(x => x.Tags, x => tags.Contains(x));

            return await _col
                .Find(filter.ToBsonDocument())
                .SortByDescending(v => v.PublishedAt)
                .Limit(take)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<uint>> GetPublishedDates(string channelId)
        {
            var result = await _col
                .Find(c => c.ChannelId == channelId)
                .Project<Video>("{ publishedAt: 1 }")
                .ToListAsync();

            return result.Select(v => v.PublishedAt).ToList();
        }

        public async Task<IReadOnlyCollection<string>> Exist(IReadOnlyCollection<string> ids)
        {
            var result = await _col
                .Find(v => ids.Contains(v.Id))
                .Project<Video>("{ _id: 1 }")
                .ToListAsync();

            return result.Select(v => v.Id).ToList();
        }

        public async Task Insert(IReadOnlyCollection<Video> videos)
        {
            await _col.InsertManyAsync(videos);
        }
    }
}
