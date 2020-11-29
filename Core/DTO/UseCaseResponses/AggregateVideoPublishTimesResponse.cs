using System;
using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public class AggregateVideoPublishTimesResponse
    {
        public Dictionary<DayOfWeek, Dictionary<int, int>> Aggregation { get; set; }

        public AggregateVideoPublishTimesResponse(Dictionary<DayOfWeek, Dictionary<int, int>> aggregation)
        {
            Aggregation = aggregation;
        }
    }
}