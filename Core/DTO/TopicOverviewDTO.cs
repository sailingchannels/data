using Core.Entities;

namespace Core.DTO
{
    public record TopicOverviewDto(
        Topic Topic, 
        string LatestVideoId, 
        string LatestChannelTitle);
}
