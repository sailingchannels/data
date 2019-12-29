using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public class TopicsOverviewResponse
    {
        public List<TopicOverviewDTO> Topics;

        public TopicsOverviewResponse(List<TopicOverviewDTO> topics)
        {
            Topics = topics;
        }
    }
}
