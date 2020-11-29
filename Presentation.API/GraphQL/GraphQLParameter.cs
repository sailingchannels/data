using System.Collections.Generic;
using System.Text.Json;

namespace Presentation.API.GraphQL
{
    public sealed class GraphQlParameter
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public Dictionary<string, JsonElement> Variables { get; set; }
    }
}