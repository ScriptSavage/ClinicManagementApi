using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    

    public AuthController(IAuthService authService,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterNewPatient([FromBody] AuthDto.RegisterNewPatient request)
    {
        var loginNumber = await _authService.RegisterNewPatient(request);
        _logger.LogInformation($"User {loginNumber} registered");
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
        var logInDate = DateTime.UtcNow;
        _logger.LogInformation($"User Has been logged in at {logInDate}");
        return Ok(accessToken);
    }

}