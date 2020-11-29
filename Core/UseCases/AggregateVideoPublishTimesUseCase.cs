using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class AggregateVideoPublishTimesUseCase : IAggregateVideoPublishTimesUseCase
    {
        private readonly IVideoRepository _videoRepository;

        public AggregateVideoPublishTimesUseCase(IVideoRepository videoRepository)
        {
            _videoRepository = videoRepository ?? throw new ArgumentNullException(nameof(videoRepository));
        }

        public async Task<AggregateVideoPublishTimesResponse> Handle(
            AggregateVideoPublishTimesRequest message
        )
        {
            var publishTimestamps = await _videoRepository.GetPublishedDates(message.ChannelId);

            var aggregation = new Dictionary<DayOfWeek, Dictionary<int, int>>();

            foreach (var timestamp in publishTimestamps)
            {
                var date = DateTimeOffset.FromUnixTimeSeconds(timestamp);

                var shouldInitDictionary = !aggregation.ContainsKey(date.DayOfWeek);
                if (shouldInitDictionary)
                {
                    aggregation[date.DayOfWeek] = new Dictionary<int, int>();
                }

                // init sub-dictionary
                var shouldInitSubDictionary = !aggregation[date.DayOfWeek].ContainsKey(date.Hour);
                if (shouldInitSubDictionary)
                {
                    aggregation[date.DayOfWeek][date.Hour] = 0;
                }

                // count days of week and hourly upload times
                aggregation[date.DayOfWeek][date.Hour]++;
            }

            return new AggregateVideoPublishTimesResponse(aggregation);
        }
    }
}
