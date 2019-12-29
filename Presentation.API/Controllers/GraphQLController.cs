using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Presentation.API.GraphQL;

namespace Presentation.API.Controllers
{
    [Route("api/graphql")]
    [ApiController]
    public class GraphQLController : Controller
    {
        private readonly ILogger<GraphQLController> _logger;
        private readonly GraphQLQuery _graphQLQuery;
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;

        public GraphQLController(
            ILogger<GraphQLController> logger, 
            GraphQLQuery graphQLQuery, 
            IDocumentExecuter documentExecuter, 
            ISchema schema)
        {
            _graphQLQuery = graphQLQuery;
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IList<GraphQLParameter> queries)
        {
            var results = new List<ExecutionResult>();

            foreach (var query in queries)
            {
                var executionOptions = new ExecutionOptions
                {
                    Schema = _schema,
                    Query = query.Query,
                    UserContext = User.Claims.ToDictionary(t => t.Type.ToString(), t => (object)t.Value),
                    Inputs = JsonSerializer.Serialize(query.Variables).ToInputs(),
                    ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 }
                };

                // execute and resolve the graphql query
                var result = await _documentExecuter.ExecuteAsync(executionOptions).ConfigureAwait(false);

                if (result.Errors?.Count > 0)
                {
                    _logger.LogError(JsonSerializer.Serialize(result.Errors));
                    return BadRequest(result.Errors);
                }

                results.Add(result);
            }

            return Ok(results);
        }
    }
}