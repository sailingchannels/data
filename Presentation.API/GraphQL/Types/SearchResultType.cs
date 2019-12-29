using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class SearchResultType
        : ObjectGraphType<SearchResultModel>, IGraphQLType
    {
        public SearchResultType()
        {
            Name = "SearchResult";

            Field<ListGraphType<ChannelType>>("Channels");
            Field<ListGraphType<VideoType>>("Videos");
        }
    }
}
