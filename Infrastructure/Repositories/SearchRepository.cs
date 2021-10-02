using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class SearchRepository : ISearchRepository
    {
        private readonly IMongoCollection<Search> _col;

        public SearchRepository(IMongoCollection<Search> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }
        
        public async Task Insert(string query)
        {
            var search = new Search
            {
                Query = query,
                Timestamp = DateTime.UtcNow
            };

            await _col.InsertOneAsync(search);
        }
    }
}