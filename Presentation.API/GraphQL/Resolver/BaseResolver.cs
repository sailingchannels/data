using System.Collections.Generic;
using Core.Entities;
using Microsoft.Toolkit.Extensions;

namespace Presentation.API.GraphQL.Resolver
{
    public abstract class BaseResolver
    {
        protected IReadOnlyCollection<T> TruncateChannelDescriptions<T>(IEnumerable<T> channels) where T : DisplayItem 
        {
            var truncatedChannels = new List<T>();
            
            foreach (var channel in channels)
            {
                var truncatedChannel = channel with { Description = channel.Description.Truncate(300, true)};
                truncatedChannels.Add(truncatedChannel);
            }

            return truncatedChannels;
        }
    }
}