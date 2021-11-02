using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Service.PublishScheduleDetection
{
    public class Worker : BackgroundService
    {
        private const int MaxRecursiveMergeNeighborsDepth = 50;

        private readonly ILogger<Worker> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly IAggregateVideoPublishTimesUseCase _aggregateVideoPublishTimesUseCase;
        private readonly IChannelPublishPredictionRepository _channelPublishPredictionRepository;

        public Worker(
            ILogger<Worker> logger,
            IChannelRepository channelRepository,
            IAggregateVideoPublishTimesUseCase aggregateVideoPublishTimesUseCase,
            IChannelPublishPredictionRepository channelPublishPredictionRepository)
        {
            _logger = logger;
            _channelRepository = channelRepository;
            _aggregateVideoPublishTimesUseCase = aggregateVideoPublishTimesUseCase;
            _channelPublishPredictionRepository = channelPublishPredictionRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // read all channel ids
            var channels = await _channelRepository.GetAllChannelIdAndTitle();
            //var channels = new List<Channel> { new() { Id = "UCBo9TLJiZ5HI5CXFsCxOhmA" } };

            _logger.LogInformation(
                $"{channels.Count} channels will be inspected for publish schedules"
            );

            foreach (var channel in channels)
            {
                try
                {
                    _logger.LogInformation(channel.Id);

                    // aggregate the channel video views of this channel per
                    // timeframes (day of week, hour of day)
                    var result = await _aggregateVideoPublishTimesUseCase.Handle(
                        new AggregateVideoPublishTimesRequest(
                            channel.Id
                        )
                    );

                    var items = new List<VideoPublishAggregationItem>();

                    foreach (var daysOfWeek in result.Aggregation)
                    {
                        items.AddRange(daysOfWeek.Value.Select(hourOfDay =>
                            new VideoPublishAggregationItem
                            {
                                DayOfTheWeek = (int)daysOfWeek.Key,
                                HourOfTheDay = hourOfDay.Key,
                                PublishedVideos = hourOfDay.Value
                            }));
                    }

                    if (items.Count == 0)
                    {
                        _logger.LogInformation($"No uploads for channel {channel.Id} within the last 4 month");
                        continue;
                    }

                    var average = items.Sum(i => i.PublishedVideos) / items.Count;

                    var sortedByDeviation = items
                        .Select(item => new PublishSchedulePrediction(
                            item,
                            item.PublishedVideos / average))
                        .OrderByDescending(item => item.DeviationFromAverage)
                        .Where(i => i.DeviationFromAverage > average)
                        .Take(5)
                        .ToList();

                    var mergedNeighbors = MergeNeighbors(sortedByDeviation);

                    if (!mergedNeighbors.Any())
                    {
                        _logger.LogWarning($"No publish schedule predictions at all for channel {channel.Id}, skipping...");
                        continue;
                    }

                    var gradient = 1 - average / mergedNeighbors.First().DeviationFromAverage;

                    var channelResult = new ChannelPublishPrediction(
                        channel.Id,
                        channel.Title,
                        average,
                        mergedNeighbors,
                        gradient,
                        DateTime.UtcNow);

                    var updatePredictionSuccessful =
                        await _channelPublishPredictionRepository.UpdatePrediction(channelResult);

                    if (!updatePredictionSuccessful)
                    {
                        _logger.LogWarning($"Updating publish prediction for channel {channel.Id} was not successful");
                    }
                    else
                    {
                        _logger.LogInformation($"New publish prediction schedule for channel {channel.Id}: gradient={gradient}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.ToString(), e.InnerException);
                }
            }
        }

        private List<PublishSchedulePrediction> MergeNeighbors(List<PublishSchedulePrediction> items, int currentDepth = 0)
        {
            if (currentDepth >= MaxRecursiveMergeNeighborsDepth)
            {
                return items;
            }

            var mergedNeighbors = new List<PublishSchedulePrediction>();
            var iterationNeighborFound = false;

            for (var i = 0; i < items.Count; i++)
            {
                var prevIndex = i - 1;

                if (prevIndex < 0)
                {
                    continue;
                }

                var prevItem = items[prevIndex];
                var currentItem = items[i];
                var neighborFound = AreDayHourNeighbors(prevItem, currentItem);

                if (neighborFound && !iterationNeighborFound)
                {
                    iterationNeighborFound = true;

                    var mergedItem = prevItem with {
                        PublishedVideos = (prevItem.PublishedVideos + currentItem.PublishedVideos) / 2,
                        DeviationFromAverage = (prevItem.DeviationFromAverage + currentItem.DeviationFromAverage) / 2
                    };

                    mergedNeighbors.Add(mergedItem);
                    i++;
                }
                else
                {
                    mergedNeighbors.Add(currentItem);
                }
            }

            if (!iterationNeighborFound)
            {
                return items;
            }

            return MergeNeighbors(mergedNeighbors, ++currentDepth);
        }

        private static bool AreDayHourNeighbors(PublishSchedulePrediction a, PublishSchedulePrediction b)
        {
            var indexDiff = Math.Abs(a.DayHourIndex - b.DayHourIndex);
            return indexDiff <= 1;
        }
    }
}
