using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using MongoDB.Driver;

namespace Infrastructure.Repositories
{
    public class ChannelPublishPredictionRepository : IChannelPublishPredictionRepository
    {
        private readonly IMongoCollection<ChannelPublishPrediction> _col;

        public ChannelPublishPredictionRepository(IMongoCollection<ChannelPublishPrediction> col)
        {
            _col = col ?? throw new ArgumentNullException(nameof(col));
        }
        
        public async Task<bool> UpdatePrediction(ChannelPublishPrediction channelPublishPrediction)
        {
            var result = await _col.ReplaceOneAsync(
                c => c.Id == channelPublishPrediction.Id, 
                channelPublishPrediction, new ReplaceOptions
                {
                    IsUpsert = true
                });
            
            var updateSuccessful = result.ModifiedCount == 1;

            return updateSuccessful;
        }
    }
}