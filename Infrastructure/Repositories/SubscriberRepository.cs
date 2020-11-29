using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly IMongoCollection<Subscriber> _col;

        public SubscriberRepository(IMongoCollection<Subscriber> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        /// <summary>
        /// Returns a historical timeseries of subscriber data
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="days"></param>
        /// <returns></returns>
        public async Task<List<Subscriber>> GetHistory(string channelId, int days)
        {
            // allow to look at a maximum of 30 days in the past
            days = Math.Min(days, 30);

            return await _col
                .Find(s => s.Id.ChannelId == channelId && s.Date >= DateTime.UtcNow.AddDays(-1 * days))
                .Project<Subscriber>("{ month: 0, year: 0, day: 0, date: 0 }")
                .SortBy(s => s.Date)
                .ToListAsync();
        }
    }
}
