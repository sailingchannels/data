using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;
using Core.Interfaces.Services;

namespace Core.UseCases
{
    public class YouTubeChannelDetailUseCase
        : IYouTubeChannelDetailUseCase
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IYouTubeDataService _youtubeDataService;
        private readonly ISailingTermRepository _sailingTermRepository;

        public YouTubeChannelDetailUseCase(
            IYouTubeDataService youtubeDataService,
            IChannelRepository channelRepository,
            ISailingTermRepository sailingTermRepository
        )
        {
            _youtubeDataService = youtubeDataService ?? throw new ArgumentNullException(nameof(youtubeDataService));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _sailingTermRepository = sailingTermRepository ?? throw new ArgumentNullException(nameof(sailingTermRepository));
        }

        public async Task<YouTubeChannelDetailResponse> Handle(YouTubeChannelDetailRequest message)
        {
            var identifiedChannels = new List<ChannelIdentificationDto>();

            var sailingTerms = await _sailingTermRepository.GetAll();
            var storedChannels = await _channelRepository.GetAll(message.ChannelIdsToCheck);

            foreach (var storedChannel in storedChannels)
            {
                identifiedChannels.Add(new ChannelIdentificationDto()
                {
                    ChannelId = storedChannel.Id,
                    Channel = storedChannel,
                    IsSailingChannel = CheckSailingTerms(sailingTerms, storedChannel),
                    Source = "db"
                });
            }

            var storedChannelIds = storedChannels.Select(c => c.Id);
            var ytFetchChannelIds = message.ChannelIdsToCheck.Except(storedChannelIds).ToList();

            var ytChannels = await _youtubeDataService.GetChannelDetails(ytFetchChannelIds);

            foreach (var ytChannel in ytChannels)
            {
                identifiedChannels.Add(new ChannelIdentificationDto()
                {
                    ChannelId = ytChannel.Id,
                    Channel = ytChannel,
                    IsSailingChannel = CheckSailingTerms(sailingTerms, ytChannel),
                    Source = "yt"
                });
            }

            var response = new YouTubeChannelDetailResponse(identifiedChannels);
            return response;
        }

        /// <summary>
        /// Checks if a channel contains sailing terms and is therefore a sailing channel
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private bool CheckSailingTerms(IReadOnlyCollection<SailingTerm> terms, DisplayItem channel)
        {
            string body = channel.Title.ToLower() + " " + channel.Description.ToLower();

            foreach (var term in terms)
            {
                if (body.Contains(term.Id))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
