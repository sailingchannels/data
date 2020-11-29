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

            string connString = "mongodb://localhost:27017";
            if(!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("MONGODB")))
            {
                connString = Environment.GetEnvironmentVariable("MONGODB");
            }

            Console.WriteLine(connString);

            // database
            services.AddSingleton<IMongoClient>(f => new MongoClient(connString));
            services.AddSingleton(f => f.GetRequiredService<IMongoClient>().GetDatabase("sailing-channels"));

            // collections
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Channel>("channels"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Search>("searches"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Video>("videos"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Tag>("tags"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Topic>("topics"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Flag>("flags"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Suggestion>("suggestions"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<SailingTerm>("sailingterms"));
            services.AddSingleton(f => f.GetRequiredService<IMongoDatabase>().GetCollection<Subscriber>("subscribers"));
        }
    }
}
