using Core.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Infrastructure.Mappings
{
    public static class MongoDBMappings
    {
        public static void AddMongoDBMappings(this IServiceCollection services)
        {
            // CHANNEL POPULARITY
            BsonClassMap.RegisterClassMap<ChannelPopularity>(cm =>
            { 
                cm.MapMember(c => c.Total).SetElementName("total");
                cm.MapMember(c => c.Views).SetElementName("views");
                cm.MapMember(c => c.Subscribers).SetElementName("subscribers");
            });

            // CHANNEL CUSTOM LINKS
            BsonClassMap.RegisterClassMap<ChannelCustomLink>(cm =>
            {
                cm.MapMember(c => c.Icon).SetElementName("icon");
                cm.MapMember(c => c.Title).SetElementName("title");
                cm.MapMember(c => c.URL).SetElementName("url");
            });

            // DISPLAY ITEM
            BsonClassMap.RegisterClassMap<DisplayItem>(cm =>
            {
                cm.MapIdMember(c => c.ID).SetElementName("_id");
                cm.MapMember(c => c.Description).SetElementName("description");
                cm.MapMember(c => c.Title).SetElementName("title");
            });

            // CHANNEL
            BsonClassMap.RegisterClassMap<Channel>(cm =>
            {
                cm.MapMember(c => c.Views).SetElementName("views");
                cm.MapMember(c => c.PublishedAt).SetElementName("publishedAt");
                cm.MapMember(c => c.LastCrawl).SetElementName("lastCrawl");
                cm.MapMember(c => c.Subscribers).SetElementName("subscribers");
                cm.MapMember(c => c.VideoCount).SetElementName("videoCount");
                cm.MapMember(c => c.Language).SetElementName("language");
                cm.MapMember(c => c.Country).SetElementName("country");
                cm.MapMember(c => c.DetectedLanguage).SetElementName("detectedLanguage");
                cm.MapMember(c => c.LastUploadAt).SetElementName("lastUploadAt");
                cm.MapMember(c => c.SubscribersHidden).SetElementName("subscribersHidden");
                cm.MapMember(c => c.Thumbnail).SetElementName("thumbnail");
                cm.MapMember(c => c.Popularity).SetElementName("popularity");
                cm.MapMember(c => c.Keywords).SetElementName("keywords");
                cm.MapMember(c => c.CustomLinks).SetElementName("customLinks");
            });

            // SEARCH
            BsonClassMap.RegisterClassMap<Search>(cm =>
            {
                cm.MapIdMember(c => c.ID).SetIdGenerator(StringObjectIdGenerator.Instance);

                cm.MapMember(c => c.Query).SetElementName("q");
                cm.MapMember(c => c.Timestamp).SetElementName("time");
            });

            // VIDEO
            BsonClassMap.RegisterClassMap<Video>(cm =>
            {
                cm.MapMember(c => c.Views).SetElementName("views");
                cm.MapMember(c => c.Dislikes).SetElementName("dislikes");
                cm.MapMember(c => c.PublishedAt).SetElementName("publishedAt");
                cm.MapMember(c => c.Likes).SetElementName("likes");
                cm.MapMember(c => c.ChannelID).SetElementName("channel");
                cm.MapMember(c => c.Comments).SetElementName("comments");
                cm.MapMember(c => c.GeoChecked).SetElementName("geoChecked");
                cm.MapMember(c => c.Tags).SetElementName("tags");
            });

            // TAG ID
            BsonClassMap.RegisterClassMap<TagID>(cm =>
            {
                cm.MapMember(c => c.ChannelID).SetElementName("channel");
                cm.MapMember(c => c.Tag).SetElementName("tag");
            }); 

            // TAG
            BsonClassMap.RegisterClassMap<Tag>(cm =>
            {
                cm.MapIdField(c => c.ID).SetElementName("_id");
                cm.MapMember(c => c.Popularity).SetElementName("value");
            });

            // TOPIC
            BsonClassMap.RegisterClassMap<Topic>(cm =>
            {
                cm.MapIdField(c => c.ID).SetElementName("_id");
                cm.MapMember(c => c.Description).SetElementName("description");
                cm.MapMember(c => c.Language).SetElementName("language");
                cm.MapMember(c => c.Title).SetElementName("title");
                cm.MapMember(c => c.SearchTerms).SetElementName("searchterms");
            });
        }
    }
}
