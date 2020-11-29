using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Service.PublishScheduleDetection.Entities;

namespace Service.PublishScheduleDetection
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IChannelRepository _channelRepository;
        private readonly IAggregateVideoPublishTimesUseCase _aggregateVideoPublishTimesUseCase;

        public Worker(
            ILogger<Worker> logger,
            IChannelRepository channelRepository,
            IAggregateVideoPublishTimesUseCase aggregateVideoPublishTimesUseCase
        )
        {
            _logger = logger;
            _channelRepository = channelRepository;
            _aggregateVideoPublishTimesUseCase = aggregateVideoPublishTimesUseCase;
        }

        /// <summary>
        /// The DetectSpike() method:
        /// - Creates the transform from the estimator.
        /// - Detects spikes based on historical sales data.
        /// - Displays the results.
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="docSize"></param>
        /// <param name="videoPublishes"></param>
        static IEnumerable<PublishSchedulePrediction> DetectSpike(
            MLContext mlContext,
            int docSize,
            IDataView videoPublishes
        )
        {
            // use the IidSpikeEstimator to train the model for spike detection
            var pipeline = mlContext.Transforms.DetectSpikeBySsa(
                outputColumnName: nameof(PublishSchedulePrediction.Prediction),
                inputColumnName: nameof(VideoPublishAggregationItem.PublishedVideos),

                confidence: 99,
                pvalueHistoryLength: docSize / 4,
                trainingWindowSize: docSize,
                seasonalityWindowSize: docSize / 4
            );

            // create the spike detection transform
            var model = pipeline.Fit(
                mlContext.Data.LoadFromEnumerable(new List<VideoPublishAggregationItem>())
            );

            // use the Transform() method to make predictions for multiple input
            // rows of a dataset
            IDataView transformedData = model.Transform(videoPublishes);

            // convert your transformedData into a strongly-typed IEnumerable for
            // easier display using the CreateEnumerable() method
            var predictions = mlContext.Data.CreateEnumerable<PublishSchedulePrediction>(
                transformedData,
                reuseRowObject: false
            );

            return predictions;
        }

        /// <summary>
        /// The main() execution routine of the worker 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // read all channel ids
            List<string> channelIds = new List<string>() { "UCZdQjaSoLjIzFnWsDQOv4ww" };//await _channelRepository.GetAllChannelIds();

            _logger.LogInformation(
                $"{channelIds.Count} channels will be inspected for publish schedules"
            );

            foreach (string channelId in channelIds)
            {
                _logger.LogInformation(channelId);

                // aggregate the channel video views of this channel per
                // timeframes (day of week, hour of day)
                var result = await _aggregateVideoPublishTimesUseCase.Handle(
                    new AggregateVideoPublishTimesRequest(
                        channelId
                    )
                );

                // prepare aggregation result data
                var items = new List<VideoPublishAggregationItem>();

                foreach (var daysOfWeek in result.Aggregation)
                {
                    foreach (var hourOfDay in daysOfWeek.Value)
                    {
                        var item = new VideoPublishAggregationItem()
                        {
                            DayOfTheWeek = (int) daysOfWeek.Key,
                            HourOfTheDay = hourOfDay.Key,
                            PublishedVideos = (float) hourOfDay.Value
                        };

                        _logger.LogInformation(JsonSerializer.Serialize(item));

                        items.Add(item);
                    }
                }

                // train ML model
                MLContext mlContext = new MLContext();
                IDataView dataView = mlContext.Data.LoadFromEnumerable<VideoPublishAggregationItem>(items);

                var predictions = DetectSpike(mlContext, items.Count, dataView);

                _logger.LogInformation("Alert\tScore\tP-Value");

                int i = 0;
                foreach (var p in predictions)
                {
                    var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}";

                    if (p.Prediction[0] == 1)
                    {
                        var input = items[i];
                        results += " <-- Spike detected";
                        results += $" - {input.DayOfTheWeek} - {input.HourOfTheDay}h";
                    }

                    i++;
                    _logger.LogInformation(results);
                }

                break;
            }
        }
    }
}
