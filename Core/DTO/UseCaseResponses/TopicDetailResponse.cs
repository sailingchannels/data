using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public class TopicDetailResponse
    {
        public Topic Topic;
        public List<Video> Videos;
        public List<Channel> Channels;
    }
}
