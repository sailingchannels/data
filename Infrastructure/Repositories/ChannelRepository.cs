using System;
using System.Collections.Generic;
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

        #region Public Methods

        /// <summary>
        /// Reads channels based on filter parameter
        /// </summary>
        /// <param name="sortBy"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="channelLanguage"></param>
        /// <returns></returns>
        public async Task<List<Channel>> GetAll(
            ChannelSorting sortBy,
            int skip = 0,
            int take = 8,
            string channelLanguage = "en"
        )
        {
            return await _col
                .Find(c => c.Language == channelLanguage)
                .Project<Channel>("{ title: 1, description: 1, lastUploadAt: 1, thumbnail: 1, views: 1, subscribers: 1, publishedAt: 1, videoCount: 1 }")
                .SortByDescending(this.getSortKey(sortBy))
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        /// <summary>
        /// Count all channels
        /// </summary>
        /// <returns></returns>
        public async Task<long> Count()
        {
            return await _col.CountDocumentsAsync(new BsonDocument());
        }

        /// <summary>
        /// Returns all channels of a certain language that are within the
        /// channelIds list
        /// </summary>
        /// <param name="channelIds"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="channelLanguage"></param>
        /// <returns></returns>
        public async Task<List<Channel>> GetAll(
            List<string> channelIds,
            int skip = 0,
            int take = 8,
            string channelLanguage = "en"
        )
        {
            return await _col
                .Find(c => c.Language == channelLanguage && channelIds.Contains(c.ID))
                .SortByDescending(c => c.LastUploadAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
        }

        /// <summary>
        /// Returns all channels that are within the hannelIds list
        /// </summary>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public async Task<List<Channel>> GetAll(List<string> channelIds)
        {
            return await _col
                .Find(c => channelIds.Contains(c.ID))
                .ToListAsync();
        }

        /// <summary>
        /// Read a single channel from database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Channel> Get(string id)
        {
            return await _col
                .Find(new BsonDocument()
                {
                    { "_id",  id}
                })
                .FirstOrDefaultAsync<Channel>();
        }

        /// <summary>
        /// Search channels by text query
        /// </summary>
        /// <param name="q"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<List<Channel>> Search(string q, string language = "en")
        {
            // guard clause
            if (string.IsNullOrWhiteSpace(q))
            {
                return new List<Channel>();
            }

            // construct the search query
            var query = new BsonDocument()
            {
                {
                    "$text", new BsonDocument()
                    {
                        { "$search", $"\"{q}\"" }
                    }
                },
                {
                    "language", language
                }
            };

            return await _col
                .Find(query)
                .Project<Channel>("{ title: 1, description: 1, lastUploadAt: 1, thumbnail: 1, views: 1, subscribers: 1, publishedAt: 1, videoCount: 1 }")
                .Limit(Constants.SEARCH_RESULTS_MAX_COUNT)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the one channel that uploaded the last from a list of channel ids
        /// Only channels that are above a certain threshold will be considered for this query
        /// </summary>
        /// <param name="channelIds"></param>
        /// <param name="subscriberMinValue"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task<Channel> GetLastUploader(
            List<string> channelIds,
            int subscriberMinValue,
            string language)
        {
            return await _col
                .Find(c => c.Language == language && c.Subscribers > subscriberMinValue && channelIds.Contains(c.ID))
                .Project<Channel>("{ title: 1 }")
                .SortByDescending(c => c.LastUploadAt)
                .FirstOrDefaultAsync();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns a Lambda expression to sort channels by based on an enum
        /// </summary>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        private Expression<Func<Channel, object>> getSortKey(ChannelSorting sortBy)
        {
            return sortBy switch
            {
                ChannelSorting.Views => (x) => x.Views,
                ChannelSorting.Founded => (x) => x.PublishedAt,
                ChannelSorting.Subscribers => (x) => x.Subscribers,
                ChannelSorting.Trending => (x) => x.Popularity.Total,
                ChannelSorting.Upload => (x) => x.LastUploadAt,
                _ => (x) => x.Subscribers
            };
        }

        #endregion
    }
}
