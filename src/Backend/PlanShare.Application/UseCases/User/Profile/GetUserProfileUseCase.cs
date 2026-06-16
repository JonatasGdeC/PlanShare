using AutoMapper;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.User.Profile;
public class GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper) : IGetUserProfileUseCase
{
    public async Task<ResponseUserProfileJson> Execute()
    {
        Domain.Entities.User user = await loggedUser.Get();

        return mapper.Map<ResponseUserProfileJson>(source: user);
    }
}