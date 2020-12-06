using System.Collections.Generic;

namespace Core.Entities
{
    public record ChannelPublishPrediction(
        string Id,
        string Title,
        float AverageUploadPerDayHour,
        IEnumerable<PublishSchedulePrediction> PredictionItems,
        float Gradient);
}