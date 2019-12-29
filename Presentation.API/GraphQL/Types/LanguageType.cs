using GraphQL.Types;
using Infrastructure.API.Models;

namespace Presentation.API.GraphQL.Types
{
    public sealed class LanguageType
        : ObjectGraphType<LanguageModel>, IGraphQLType
    {
        public LanguageType()
        {
            Name = "Language";

            Field(i => i.Name, nullable: false);
            Field(i => i.Code, nullable: false);
        }
    }
}
