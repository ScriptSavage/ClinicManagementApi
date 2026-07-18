using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterNewPatient([FromBody] AuthDto.RegisterNewPatient request)
    {
        var loginNumber = await _authService.RegisterNewPatient(request);
        return Ok(new { 
            message = "Account created successfully",
            login = loginNumber 
        });
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto.LoginDto loginRequest)
    {
        var accessToken = await _authService.GenerateAccessToken(loginRequest);
        return Ok(accessToken);
    }

}