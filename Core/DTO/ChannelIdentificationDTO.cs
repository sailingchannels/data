using System;
using Core.Entities;
using Core.Enums;

namespace Core.DTO
{
    public class ChannelIdentificationDTO
    {
        public string ChannelId;
        public string Source;
        public bool IsSailingChannel;
        public DisplayItem Channel;
        public ChannelIdentificationStatus Status;
    }
}
