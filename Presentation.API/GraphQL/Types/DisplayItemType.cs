using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class DisplayItemType
        : ObjectGraphType<DisplayItemModel>, IGraphQLType
    {
        public DisplayItemType()
        {
            Name = "DisplayItem";

            Field(i => i.ID);
            Field(i => i.Title, true);
            Field(i => i.Description, true);
            Field(i => i.Thumbnail, true);
        }
    }
}
