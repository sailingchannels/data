using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public record TopicDetailResponse(
        Topic Topic, 
        IReadOnlyCollection<Video> Videos, 
        IReadOnlyCollection<Channel> Channels);
}
