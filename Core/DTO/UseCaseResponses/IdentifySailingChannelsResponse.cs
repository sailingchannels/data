using Core.Enums;

namespace Core.DTO.UseCaseResponses
{
    public record IdentifySailingChannelResponse
    {
        public ChannelIdentificationStatus Status { get; init; }
        public ChannelIdentificationDto IdentifiedChannel { get; init; }

        public IdentifySailingChannelResponse(
            ChannelIdentificationStatus status
        )
        {
            Status = status;
            IdentifiedChannel = new ChannelIdentificationDto() with { Status = status };
        }

        public IdentifySailingChannelResponse(
            ChannelIdentificationStatus status,
            ChannelIdentificationDto identifiedChannel
        )
        {
            Status = status;
            IdentifiedChannel = identifiedChannel with { Status = status };
        }
    }
}
