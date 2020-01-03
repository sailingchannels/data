using Core.Enums;

namespace Core.DTO.UseCaseResponses
{
    public class IdentifySailingChannelResponse
    {
        public ChannelIdentificationStatus Status { get; set; }
        public ChannelIdentificationDTO IdentifiedChannel { get; set; }

        public IdentifySailingChannelResponse(
            ChannelIdentificationStatus status
        )
        {
            Status = status;
            IdentifiedChannel = new ChannelIdentificationDTO()
            {
                Status = status
            };
        }

        public IdentifySailingChannelResponse(
            ChannelIdentificationStatus status,
            ChannelIdentificationDTO identifiedChannel
        )
        {
            Status = status;
            identifiedChannel.Status = status;
            IdentifiedChannel = identifiedChannel;
        }
    }
}
