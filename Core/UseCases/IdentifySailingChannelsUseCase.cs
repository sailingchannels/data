using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Enums;
using Core.Interfaces.UseCases;
using Core.DTO;
using Core.Interfaces.Repositories;

namespace Core.UseCases
{
    public class IdentifySailingChannelUseCase : IIdentifySailingChannelUseCase
    {
        private readonly IExtractYouTubeChannelIDUseCase _extractYouTubeChannelIDUseCase;
        private readonly IYouTubeChannelDetailUseCase _youTubeChannelDetailUseCase;
        private readonly ISuggestionRepository _suggestionRepository;

        public IdentifySailingChannelUseCase(
            IExtractYouTubeChannelIDUseCase extractYouTubeChannelIDUseCase,
            IYouTubeChannelDetailUseCase youTubeChannelDetailUseCase,
            ISuggestionRepository suggestionRepository
        )
        {
            _extractYouTubeChannelIDUseCase = extractYouTubeChannelIDUseCase ?? throw new ArgumentNullException(nameof(extractYouTubeChannelIDUseCase));
            _youTubeChannelDetailUseCase = youTubeChannelDetailUseCase ?? throw new ArgumentNullException(nameof(youTubeChannelDetailUseCase));
            _suggestionRepository = suggestionRepository ?? throw new ArgumentNullException(nameof(suggestionRepository));
        }

        public async Task<IdentifySailingChannelResponse> Handle(IdentifySailingChannelRequest message)
        {
            // guard clause
            if (string.IsNullOrWhiteSpace(message.PossibleYouTubeChannelURL))
            {
                return new IdentifySailingChannelResponse(
                    ChannelIdentificationStatus.NotValid
                );
            }

            // try to identify a channel id from the URL
            var channelIDResponse = await _extractYouTubeChannelIDUseCase.Handle(
                new ExtractYouTubeChannelIDRequest(message.PossibleYouTubeChannelURL)
            );

            // could not identy a channel
            if (string.IsNullOrWhiteSpace(channelIDResponse.ChannelID))
            {
                return new IdentifySailingChannelResponse(
                    ChannelIdentificationStatus.NotValid
                );
            }

            // fetch details for youtube channel
            var channelDetails = await _youTubeChannelDetailUseCase.Handle(
                new YouTubeChannelDetailRequest(
                    new List<string>() { channelIDResponse.ChannelID }
                )
            );

            if (channelDetails.IdentifiedChannels?.Count == 0)
            {
                return new IdentifySailingChannelResponse(
                    ChannelIdentificationStatus.NotValid
                );
            }

            ChannelIdentificationDTO channelDetail = channelDetails.IdentifiedChannels.First();

            // we discovered a newly listed channel
            if (channelDetail.Source.ToLower() == "yt")
            {
                // check if channel has been suggested before
                var suggestions = await _suggestionRepository.GetAny(
                    new List<string>() { channelIDResponse.ChannelID }
                );

                // a suggestion does already exist for this channel
                if (suggestions.Count > 0)
                {
                    return new IdentifySailingChannelResponse(
                        ChannelIdentificationStatus.AlreadySuggested
                    );
                }

                // we found a novel channel at this point!
                return new IdentifySailingChannelResponse(
                    ChannelIdentificationStatus.Novel,
                    channelDetail
                );
            }

            // this channel must already be listed
            return new IdentifySailingChannelResponse(
                ChannelIdentificationStatus.AlreadyListed,
                channelDetail
            );
        }
    }
}
