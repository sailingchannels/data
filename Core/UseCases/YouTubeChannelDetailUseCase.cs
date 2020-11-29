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
            var response = new YouTubeChannelDetailResponse();

            // read all sailing terms from database
            List<SailingTerm> sailingTerms = await _sailingTermRepository.GetAll();

            // try to fetch channels from database, this will likely return a subset of the channel ids
            // requested, since we might not have any channel in our database
            List<Channel> dbChannels = await _channelRepository.GetAll(message.ChannelIdsToCheck);

            // add database channels to result
            foreach (var dbChannel in dbChannels)
            {
                response.IdentifiedChannels.Add(new ChannelIdentificationDTO()
                {
                    ChannelId = dbChannel.ID,
                    Channel = dbChannel,
                    IsSailingChannel = CheckSailingTerms(sailingTerms, dbChannel),
                    Source = "db"
                });
            }

            // extract a list of channel ids from the database channels, this we will compare to the channels
            // that we were supposed to check and determine the channel ids that we'll have to fetch from
            // youtube api
            IEnumerable<string> dbChannelIds = dbChannels.Select(c => c.ID);

            // determine the youtube channels to fetch via youtube api, by checking which channels from the
            // channelIdsToCheck have not been covered in the dbChannelIds list
            IEnumerable<string> ytFetchChannelIds = message.ChannelIdsToCheck.Except(dbChannelIds);

            // fetch missing channels from youtube
            IEnumerable<YouTubeChannel> ytChannels = await _youtubeDataService.GetChannelDetails(ytFetchChannelIds);

            // add youtube channels to result
            foreach (var ytChannel in ytChannels)
            {
                response.IdentifiedChannels.Add(new ChannelIdentificationDTO()
                {
                    ChannelId = ytChannel.ID,
                    Channel = ytChannel,
                    IsSailingChannel = CheckSailingTerms(sailingTerms, ytChannel),
                    Source = "yt"
                });
            }

            return response;
        }

        /// <summary>
        /// Checks if a channel contains sailing terms and is therefore a sailing channel
        /// </summary>
        /// <param name="terms"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        private bool CheckSailingTerms(List<SailingTerm> terms, DisplayItem channel)
        {
            string body = channel.Title.ToLower() + " " + channel.Description.ToLower();

            foreach (var term in terms)
            {
                if (body.Contains(term.ID))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
