using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class SailingTermRepository : ISailingTermRepository
    {
        private readonly IMongoCollection<SailingTerm> _col;

        public SailingTermRepository(IMongoCollection<SailingTerm> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }

        #region Public Methods

        /// <summary>
        /// Gets a list of all sailing terms
        /// </summary>
        /// <returns></returns>
        public async Task<List<SailingTerm>> GetAll()
        {
            return await _col
                .Find(new BsonDocument())
                .ToListAsync();
        }

        #endregion
    }
}
