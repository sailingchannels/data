using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.UseCases;
using Core.Entities;
using Core.Interfaces.Repositories;
using System;
using Core.DTO;

namespace Core.UseCases
{
    public class ChannelSuggestionsUseCase
        : IChannelSuggestionsUseCase
    {

        private readonly IYouTubeChannelDetailUseCase _youTubeChannelDetailUseCase;
        private readonly ISuggestionRepository _suggestionRepository;

        public ChannelSuggestionsUseCase(
            ISuggestionRepository suggestionRepository,
            IYouTubeChannelDetailUseCase youTubeChannelDetailUseCase
        )
        {
            _suggestionRepository = suggestionRepository ?? throw new ArgumentNullException("suggestionRepository");
            _youTubeChannelDetailUseCase = youTubeChannelDetailUseCase ?? throw new ArgumentNullException("youTubeChannelDetailUseCase");
        }

        public async Task<ChannelSuggestionsResponse> Handle(ChannelSuggestionsRequest message)
        {
            var result = await _youTubeChannelDetailUseCase.Handle(
                new YouTubeChannelDetailRequest(
                    message.ChannelIds
                )
            );

            List<ChannelIdentificationDTO> channels = result.IdentifiedChannels;

            // if source filter is specified, only return matching channels
            if (!string.IsNullOrWhiteSpace(message.Source))
            {
                channels = channels.Where(i => i.Source == message.Source).ToList();
            }

            // get a list of all channel ids of the results
            List<string> resultChannelIds = channels.Select(c => c.ChannelId).ToList();

            // fetch possible suggestions of these channel ids
            if(!string.IsNullOrWhiteSpace(message.UserId))
            {
                List<Suggestion> suggestions = await _suggestionRepository.GetAny(message.UserId, resultChannelIds);
                List<string> suggestionChannelIds = suggestions.Select(s => s.ID.ChannelID).ToList();

                // only include channels that are not already suggested by this user
                channels = channels.Where(c => !suggestionChannelIds.Contains(c.ChannelId)).ToList();
            }

            return new ChannelSuggestionsResponse(channels);
        }
    }
}
