using System.Collections.Generic;

namespace Core.DTO.UseCaseResponses
{
    public record TopicsOverviewResponse(IReadOnlyCollection<TopicOverviewDto> Topics);
}
