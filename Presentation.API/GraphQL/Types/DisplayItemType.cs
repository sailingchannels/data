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

            Field(i => i.ID, nullable: false);
            Field(i => i.Title, nullable: true);
            Field(i => i.Description, nullable: true);
            Field(i => i.Thumbnail, nullable: true);
        }
    }
}
