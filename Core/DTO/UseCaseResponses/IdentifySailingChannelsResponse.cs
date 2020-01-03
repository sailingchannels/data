using System;
using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public class IdentifySailingChannelsResponse
    {
        public List<string> Rejected { get; set; } = new List<string>();
        public List<ChannelIdentificationDTO> IdentifiedChannels = new List<ChannelIdentificationDTO>();

        public IdentifySailingChannelsResponse()
        {
        }
    }
}
