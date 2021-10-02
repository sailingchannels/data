using System;
using System.Collections.Generic;
using AutoMapper;
using Core.Interfaces.Repositories;
using GraphQL.Types;
using Infrastructure.API.Models;
using Presentation.API.GraphQL.Types;

namespace Presentation.API.GraphQL.Resolver
{
    public sealed class LanguageResolver
        : IResolver, ILanguageResolver
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly IMapper _mapper;

        public LanguageResolver(
            ILanguageRepository languageRepository,
            IMapper mapper
        )
        {
            _languageRepository = languageRepository ?? throw new ArgumentNullException("languageRepository");
            _mapper = mapper ?? throw new ArgumentNullException("mapper");
        }

        /// <summary>
        /// Resolves all queries on guest accesses
        /// </summary>
        /// <param name="graphQlQuery"></param>
        public void ResolveQuery(GraphQlQuery graphQlQuery)
        {
            // LANGUAGES: a list of all lang codes
            graphQlQuery.FieldAsync<ListGraphType<LanguageType>>(
                "languages",
                resolve: async context =>
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
        /// <param name="graphQlMutation"></param>
        public void ResolveMutation(GraphQlMutation graphQlMutation)
        {
        }
    }
}
