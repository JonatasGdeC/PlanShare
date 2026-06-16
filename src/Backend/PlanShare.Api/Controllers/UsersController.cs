using Microsoft.AspNetCore.Mvc;
using PlanShare.Application.UseCases.User.ChangePassword;
using PlanShare.Application.UseCases.User.Profile;
using PlanShare.Application.UseCases.User.Register;
using PlanShare.Application.UseCases.User.Update;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;

namespace PlanShare.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(type: typeof(ResponseRegisteredUserJson), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromServices] IRegisterUserUseCase useCase, [FromBody] RequestRegisterUserJson request)
    {
        ResponseRegisteredUserJson response = await useCase.Execute(request: request);

        return Created(uri: string.Empty, value: response);
    }

    [HttpPut]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request: request);

        return NoContent();
    }

    [HttpGet]
    [ProducesResponseType(type: typeof(ResponseRegisteredUserJson), statusCode: StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
    {
        ResponseUserProfileJson response = await useCase.Execute();

        return Ok(value: response);
    }

    [HttpPut(template: "change-password")]
    [ProducesResponseType(statusCode: StatusCodes.Status204NoContent)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request: request);

        return NoContent();
    }
}
