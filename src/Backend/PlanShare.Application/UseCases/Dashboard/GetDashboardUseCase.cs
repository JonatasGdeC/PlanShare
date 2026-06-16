using AutoMapper;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.Association;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;

namespace PlanShare.Application.UseCases.Dashboard;
public class GetDashboardUseCase : IGetDashboardUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    private readonly IWorkItemReadOnlyRepository _workItemRepository;
    private readonly IPersonAssociationReadOnlyRepository _personAssociationRepository;

    public GetDashboardUseCase(ILoggedUser loggedUser, IWorkItemReadOnlyRepository workItemRepository, IMapper mapper, IPersonAssociationReadOnlyRepository personAssociationRepository)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
        _workItemRepository = workItemRepository;
        _personAssociationRepository = personAssociationRepository;
    }

    public async Task<ResponseDashboardJson> Execute()
    {
        Domain.Entities.User loggedUser = await _loggedUser.Get();

        List<Domain.Entities.WorkItem> workItems = await _workItemRepository.GetAll(user: loggedUser);
        List<Domain.Entities.User> associations = await _personAssociationRepository.GetPersonAssociationsForUser(user: loggedUser);

        return new ResponseDashboardJson
        {
            WorkItems = _mapper.Map<List<ResponseShortWorkItemJson>>(source: workItems),
            Friends = _mapper.Map<List<ResponseAssigneeJson>>(source: associations)
        };
    }
}
