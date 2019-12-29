using System;
using Core.Entities;
using Infrastructure.Mappings;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Tag = Core.Entities.Tag;

namespace Infrastructure
{
    public static class MongoDB
    {
        public static void AddMongoDB(this IServiceCollection services)
        {
            // entity mappings
            services.AddMongoDBMappings();

            // database
            services.AddSingleton<IMongoClient>(f => new MongoClient(Environment.GetEnvironmentVariable("MONGODB")));
            services.AddSingleton(f => f.GetRequiredService<IMongoClient>().GetDatabase("sailing-channels"));

            // collections
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Channel>("channels"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Search>("searches"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Video>("videos"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Tag>("tags"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Topic>("topics"));
        }
    }
}
