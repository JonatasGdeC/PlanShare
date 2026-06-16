using AutoMapper;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.Association;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.Dashboard;
public class GetDashboardUseCase(
    ILoggedUser user,
    IWorkItemReadOnlyRepository workItemRepository,
    IMapper mapper,
    IPersonAssociationReadOnlyRepository personAssociationRepository)
    : IGetDashboardUseCase
{
    public async Task<ResponseDashboardJson> Execute()
    {
        Domain.Entities.User loggedUser = await user.Get();

        List<Domain.Entities.WorkItem> workItems = await workItemRepository.GetAll(user: loggedUser);
        List<Domain.Entities.User> associations = await personAssociationRepository.GetPersonAssociationsForUser(user: loggedUser);

        return new ResponseDashboardJson
        {
            WorkItems = mapper.Map<List<ResponseShortWorkItemJson>>(source: workItems),
            Friends = mapper.Map<List<ResponseAssigneeJson>>(source: associations)
        };
    }
}
