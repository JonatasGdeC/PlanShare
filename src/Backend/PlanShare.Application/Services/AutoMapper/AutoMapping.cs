using AutoMapper;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;

namespace PlanShare.Application.Services.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestRegisterUserJson, Domain.Entities.User>()
            .ForMember(destinationMember: dest => dest.Password, memberOptions: opt => opt.Ignore());

        CreateMap<RequestRegisterWorkItemJson, Domain.Entities.WorkItem>()
            .ForMember(destinationMember: dest => dest.DueDate, memberOptions: opt => opt.MapFrom(mapExpression: source => source.DueDate.Date))
            .ForMember(destinationMember: dest => dest.Assignees, memberOptions: opt => opt.MapFrom(mapExpression: source => source.Assignees.Distinct()));

        CreateMap<Guid, Domain.Entities.Assignee>()
            .ForMember(destinationMember: dest => dest.UserId, memberOptions: opt => opt.MapFrom(mapExpression: source => source));

        CreateMap<RequestUpdateWorkItemJson, Domain.Entities.WorkItem>()
            .ForMember(destinationMember: dest => dest.DueDate, memberOptions: opt => opt.MapFrom(mapExpression: source => source.DueDate.Date));
    }

    private void DomainToResponse()
    {
        CreateMap<Domain.Entities.User, ResponseUserProfileJson>();

        CreateMap<Domain.Entities.User, ResponseAssigneeJson>();

        CreateMap<Domain.Entities.WorkItem, ResponseShortWorkItemJson>();
    }
}
