using System;
using System.Linq;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;
using Core.Interfaces.Repositories;
using Core.Interfaces.UseCases;

namespace Core.UseCases
{
    public class ChannelSuggestionsUseCase : IChannelSuggestionsUseCase
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

            var channels = result.IdentifiedChannels;

            var sourceFilterIsSpecified = !string.IsNullOrWhiteSpace(message.Source);
            if (sourceFilterIsSpecified)
            {
                channels = channels.Where(i => i.Source == message.Source).ToList();
            }
            
            var resultChannelIds = channels.Select(c => c.ChannelId).ToList();

            // fetch possible suggestions of these channel ids
            var userIdIsSpecified = !string.IsNullOrWhiteSpace(message.UserId);
            if (userIdIsSpecified)
            {
                var suggestions = await _suggestionRepository.GetAny(message.UserId, resultChannelIds);
                var suggestionChannelIds = suggestions.Select(s => s.Id.ChannelId).ToList();

                channels = channels
                    .Where(c => !suggestionChannelIds.Contains(c.ChannelId))
                    .ToList();
            }

            return new ChannelSuggestionsResponse(channels);
        }
    }
}
