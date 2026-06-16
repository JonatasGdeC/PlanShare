using Microsoft.AspNetCore.Mvc;
using PlanShare.Application.UseCases.Login.DoLogin;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;

namespace PlanShare.Api.Controllers;

[Route(template: "[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(type: typeof(ResponseRegisteredUserJson), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ResponseErrorJson), statusCode: StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        ResponseRegisteredUserJson response = await useCase.Execute(request: request);

        return Ok(value: response);
    }
}
