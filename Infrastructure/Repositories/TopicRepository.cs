using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public sealed class TopicRepository : ITopicRepository
    {
        private readonly IMongoCollection<Topic> _col;

        public TopicRepository(IMongoCollection<Topic> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }
        
        public async Task<IReadOnlyCollection<Topic>> GetAll(string language)
        {
            return await _col
                .Find(t => t.Language == language)
                .ToListAsync();
        }

        public async Task<Topic> Get(string id)
        {
            return await _col
                .Find(t => t.Id == id)
                .SingleOrDefaultAsync();
        }
    }
}
