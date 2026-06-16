using AutoMapper;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.WorkItem.GetAll;
public class GetAllWorkItemUseCase(
    ILoggedUser user,
    IMapper mapper,
    IWorkItemReadOnlyRepository repository)
    : IGetAllWorkItemUseCase
{
    public async Task<ResponseWorkItemsJson> Execute()
    {
        Domain.Entities.User loggedUser = await user.Get();

        List<Domain.Entities.WorkItem> workItem = await repository.GetAll(user: loggedUser);

        return new ResponseWorkItemsJson
        {
            WorkItems = mapper.Map<List<ResponseShortWorkItemJson>>(source: workItem)
        };
    }
}
