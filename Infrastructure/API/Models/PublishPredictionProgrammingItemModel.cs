using System.Collections.Generic;

namespace Infrastructure.API.Models
{
    public record PublishPredictionProgrammingItemModel(
        int DayOfTheWeek,
        int HourOfTheDay,
        IReadOnlyCollection<ChannelModel> Channels);
}