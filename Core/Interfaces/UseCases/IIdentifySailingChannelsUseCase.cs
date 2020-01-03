using Core.DTO.UseCaseRequests;
using Core.DTO.UseCaseResponses;

namespace Core.Interfaces.UseCases
{
    public interface IIdentifySailingChannelsUseCase
        : IUseCase<IdentifySailingChannelsRequest, IdentifySailingChannelsResponse>
    {
    }
}
