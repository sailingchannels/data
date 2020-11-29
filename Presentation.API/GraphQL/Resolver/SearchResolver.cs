using System;
using AutoMapper;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.UseCases;
using GraphQL.Types;
using Infrastructure.API.Models;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class SearchResolver
        : BaseResolver, IResolver, ISearchResolver
    {
        private readonly ISearchUseCase _searchUseCase;
        private readonly IMapper _mapper;

        public SearchResolver(
            ISearchUseCase searchUseCase,
            IMapper mapper
        )
        {
            _searchUseCase = searchUseCase ?? throw new ArgumentNullException("searchUseCase");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // LANGUAGES: a list of all lang codes
            graphQlQuery.FieldAsync<SearchResultType>(
                "searchResults",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "query" }
                ),
                resolve: async (context) =>
                {
                    var searchResult = await _searchUseCase.Handle(new SearchRequest(
                        context.GetArgument<string>("query")
                    ));
                    
                    var channelsWithTruncatedDescription = 
                        TruncateChannelDescriptions(searchResult.Channels);

                    var truncatedDescriptionResults = searchResult with {
                        Channels = channelsWithTruncatedDescription
                    };

                    // map entity to model
                    return _mapper.Map<SearchResultModel>(truncatedDescriptionResults);
                }
            );
        }

        /// <summary>
        /// Resolves all mutations on guest accesses.
        /// </summary>
        /// <param name="graphQlMutation"></param>
        public void ResolveMutation(GraphQlMutation graphQlMutation)
        {
        }
    }
}
