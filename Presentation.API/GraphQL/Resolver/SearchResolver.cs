using System;
using System.Collections.Generic;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Toolkit.Extensions;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class SearchResolver
        : IResolver, ISearchResolver
    {
        private readonly ISearchUseCase _searchUseCase;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchResolver> _logger;

        public SearchResolver(
            ILogger<SearchResolver> logger,
            ISearchUseCase searchUseCase,
            IMapper mapper
        )
        {
            _searchUseCase = searchUseCase ?? throw new ArgumentNullException("searchUseCase");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
            _logger = logger;
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQLQuery"></param>
        public void ResolveQuery(GraphQLQuery graphQLQuery)
        {
            // LANGUAGES: a list of all lang codes
            graphQLQuery.FieldAsync<SearchResultType>(
                "searchResults",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" }
                ),
                resolve: async (context) =>
                {
                    var searchResult = await _searchUseCase.Handle(new SearchRequest(
                        context.GetArgument<string>("query")
                    ));

                    // truncate description
                    foreach (var channel in searchResult.Channels)
                    {
                        channel.Description = channel.Description.Truncate(300, ellipsis: true);
                    }

                    // map entity to model
                    return _mapper.Map<SearchResultModel>(searchResult);
                }
            );
        }

        /// <summary>
        /// Resolves all mutations on guest accesses.
        /// </summary>
        /// <param name="graphQLMutation"></param>
        public void ResolveMutation(GraphQLMutation graphQLMutation)
        {
        }
    }
}
