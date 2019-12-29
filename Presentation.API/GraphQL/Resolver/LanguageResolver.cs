using System;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Infrastructure.API.Models;
using Microsoft.Extensions.Logging;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class LanguageResolver
        : IResolver, ILanguageResolver
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LanguageResolver> _logger;

        public LanguageResolver(
            ILogger<LanguageResolver> logger,
            ILanguageRepository languageRepository,
            IMapper mapper
        )
        {
            _languageRepository = languageRepository ?? throw new ArgumentNullException("languageRepository");
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
            graphQLQuery.FieldAsync<ListGraphType<LanguageType>>(
                "languages",
                resolve: async (context) =>
                {
                    var languages = await _languageRepository.GetAll();

                    // map entity to model
                    return _mapper.Map<List<LanguageModel>>(languages);
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
