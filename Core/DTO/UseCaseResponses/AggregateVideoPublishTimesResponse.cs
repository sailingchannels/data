using System;
using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public record AggregateVideoPublishTimesResponse(
        IReadOnlyDictionary<DayOfWeek, Dictionary<int, int>> Aggregation);
}