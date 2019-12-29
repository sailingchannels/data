using System.Collections.Generic;
using Core.Entities;

namespace Core.DTO.UseCaseResponses
{
    public class SearchResponse
    {
        public List<Video> Videos;
        public List<Channel> Channels;
    }
}
