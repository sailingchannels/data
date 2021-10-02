using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class ChannelRepository : IChannelRepository
    {
        private readonly IMongoCollection<Channel> _col;

        public ChannelRepository(IMongoCollection<Channel> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        public async Task<IReadOnlyCollection<Channel>> GetAll(
            ChannelSorting sortBy,
            int skip = 0,
            int take = 8,
            string channelLanguage = "en"
        )
        {
            return await _col
                .Find(c => c.Language == channelLanguage)
                .Project<Channel>("{ title: 1, description: 1, lastUploadAt: 1, thumbnail: 1, views: 1, subscribers: 1, publishedAt: 1, videoCount: 1 }")
                .SortByDescending(getSortKey(sortBy))
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Channel>> GetAllChannelIdAndTitle()
        {
            var channels = await _col
                .Find(new BsonDocument())
                .Project<Channel>("{ _id: 1, title: 1 }")
                .ToListAsync();

            return channels;
        }

        public async Task<long> Count()
        {
            return await _col.CountDocumentsAsync(new BsonDocument());
        }

        public async Task<IReadOnlyCollection<Channel>> GetAll(
            IReadOnlyCollection<string> channelIds,
            int skip = 0,
            int take = 8,
            string channelLanguage = "en"
        )
        {
            return await _col
                .Find(c => c.Language == channelLanguage && channelIds.Contains(c.Id))
                .SortByDescending(c => c.LastUploadAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Channel>> GetAll(IReadOnlyCollection<string> channelIds)
        {
            return await _col
                .Find(c => channelIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<Channel> Get(string id)
        {
            return await _col
                .Find(new BsonDocument
                {
                    { "_id",  id}
                })
                .FirstOrDefaultAsync<Channel>();
        }

        public async Task<IReadOnlyCollection<Channel>> Search(string q, string language = "en")
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return new List<Channel>();
            }

            var searchQuery = new BsonDocument
            {
                {
                    "$text", new BsonDocument
                    {
                        { "$search", $"\"{q}\"" }
                    }
                },
                {
                    "language", language
                }
            };

            return await _col
                .Find(searchQuery)
                .Project<Channel>("{ title: 1, description: 1, lastUploadAt: 1, thumbnail: 1, views: 1, subscribers: 1, publishedAt: 1, videoCount: 1 }")
                .Limit(Constants.SEARCH_RESULTS_MAX_COUNT)
                .ToListAsync();
        }

        public async Task<Channel> GetLastUploader(
            IReadOnlyCollection<string> channelIds,
            int subscriberMinValue,
            string language)
        {
            return await _col
                .Find(c => c.Language == language && c.Subscribers > subscriberMinValue && channelIds.Contains(c.Id))
                .Project<Channel>("{ title: 1 }")
                .SortByDescending(c => c.LastUploadAt)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Channel>> GetIDsByLastUploadTimerange(
           DateTime from,
           DateTime until
        )
        {
            return await _col
                .Find(c =>
                    c.LastUploadAt <= new DateTimeOffset(from).ToUnixTimeSeconds() &&
                    c.LastUploadAt > new DateTimeOffset(until).ToUnixTimeSeconds()
                )
                .Project<Channel>("{ _id: 1 }")
                .ToListAsync();
        }

        public async Task UpdateNewVideoWasFound(string channelId, uint lastUploadTimestamp)
        {
            var update = Builders<Channel>.Update
                .Set(c => c.LastUploadAt, lastUploadTimestamp)
                .Set(c => c.LastCrawl, DateTime.UtcNow);

            await _col.UpdateOneAsync(c => c.Id == channelId, update);
        }

        public async Task<uint> GetLastUploadTimestamp(string channelId)
        {
            var channel = await _col
                .Find(c => c.Id == channelId)
                .Project<Channel>("{ lastUploadAt: 1 }")
                .SingleOrDefaultAsync();

            return channel.LastUploadAt ?? 0;
        }

        private Expression<Func<Channel, object>> getSortKey(ChannelSorting sortBy)
        {
            return sortBy switch
            {
                ChannelSorting.Views => x => x.Views,
                ChannelSorting.Founded => x => x.PublishedAt,
                ChannelSorting.Subscribers => x => x.Subscribers,
                ChannelSorting.Trending => x => x.Popularity.Total,
                ChannelSorting.Upload => x => x.LastUploadAt,
                _ => x => x.Subscribers
            };
        }
    }
}
