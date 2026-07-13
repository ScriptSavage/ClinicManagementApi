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
        await _authService.RegisterNewPatient(request);
        return Ok(new { Message = "Registration Successful" });
    }
}