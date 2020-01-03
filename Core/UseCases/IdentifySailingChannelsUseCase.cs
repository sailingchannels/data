using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class IdentifySailingChannelsUseCase : IIdentifySailingChannelsUseCase
    {
        private readonly IExtractYouTubeChannelIDUseCase _extractYouTubeChannelIDUseCase;
        private readonly IChannelSuggestionsUseCase _channelSuggestionsUseCase;

        public IdentifySailingChannelsUseCase(
            IExtractYouTubeChannelIDUseCase extractYouTubeChannelIDUseCase,
            IChannelSuggestionsUseCase channelSuggestionsUseCase
        )
        {
            _extractYouTubeChannelIDUseCase = extractYouTubeChannelIDUseCase ?? throw new ArgumentNullException(nameof(extractYouTubeChannelIDUseCase));
            _channelSuggestionsUseCase = channelSuggestionsUseCase ?? throw new ArgumentNullException(nameof(channelSuggestionsUseCase));
        }

        public async Task<IdentifySailingChannelsResponse> Handle(IdentifySailingChannelsRequest message)
        {
            var response = new IdentifySailingChannelsResponse();

            // guard clause
            if(message?.PossibleYouTubeChannelURLs?.Count == 0)
            {
                return response;
            }

            var channelIdsToCheck = new List<string>();

            foreach (var possibleYoutubeChannelURL in message.PossibleYouTubeChannelURLs)
            {
                var channelIDResponse = await _extractYouTubeChannelIDUseCase.Handle(
                    new ExtractYouTubeChannelIDRequest(possibleYoutubeChannelURL)
                );

                // could not identy a channel
                if (string.IsNullOrWhiteSpace(channelIDResponse.ChannelID))
                {
                    response.Rejected.Add(possibleYoutubeChannelURL);
                }
                else
                {
                    channelIdsToCheck.Add(possibleYoutubeChannelURL);
                }
            }

            // acquire channel details
            var details = await _channelSuggestionsUseCase.Handle(
                new ChannelSuggestionsRequest(channelIdsToCheck, message.UserId, null)
            );

            response.IdentifiedChannels = details.Suggestions;

            return response;
        }
    }
}
