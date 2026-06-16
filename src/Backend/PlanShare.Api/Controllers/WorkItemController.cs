using Microsoft.AspNetCore.Mvc;
using PlanShare.Application.UseCases.WorkItem.Delete;
using PlanShare.Application.UseCases.WorkItem.GetAll;
using PlanShare.Application.UseCases.WorkItem.GetById;
using PlanShare.Application.UseCases.WorkItem.Register;
using PlanShare.Application.UseCases.WorkItem.Update;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;

namespace PlanShare.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
public class WorkItemController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(type: typeof(ResponseRegisteredWorkItemJson), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterWorkItemUseCase useCase,
        [FromForm] RequestRegisterWorkItemJson request,
        List<IFormFile> files)
    {
        ResponseRegisteredWorkItemJson response = await useCase.Execute(request: request);

        return Created(uri: string.Empty, value: response);
    }

    [HttpDelete]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromServices] IDeleteWorkItemUseCase useCase, [FromRoute] Guid id)
    {
        await useCase.Execute(workItemId: id);

        return NoContent();
    }

    [HttpGet]
    [Route(template: "{id}")]
    [ProducesResponseType(type: typeof(ResponseWorkItemJson), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromServices] IGetByIdWorkItemUseCase useCase, [FromRoute] Guid id)
    {
        ResponseWorkItemJson result = await useCase.Execute(workItemId: id);

        return Ok(value: result);
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(ResponseWorkItemJson), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllWorkItemUseCase useCase)
    {
        ResponseWorkItemsJson result = await useCase.Execute();
        if(result.WorkItems.Count == 0)
            return NoContent();

        return Ok(value: result);
    }

    [HttpPut]
    [Route(template: "{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromServices] IUpdateWorkItemUseCase useCase, [FromRoute] Guid id, [FromBody] RequestUpdateWorkItemJson request)
    {
        await useCase.Execute(workItemId: id, request: request);

        return NoContent();
    }
}
