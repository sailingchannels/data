using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public record SearchResponse(IReadOnlyCollection<Video> Videos, IReadOnlyCollection<Channel> Channels);
}
