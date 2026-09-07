using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationCore.Auth.Dto;
using ApplicationCore.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/auth")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
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

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] AuthDto.LoginDto loginRequest)
    {
        var access = await _authService.LoginAsync(loginRequest);
        return Ok(access);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] AuthDto.RefreshTokenDto request)
    {
        return Ok(await _authService.RefreshAsync(request));
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] AuthDto.RefreshTokenDto request)
    {
        await _authService.LogoutAsync(request);
        return NoContent();
    }


    [Authorize]
    [HttpPatch("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] AuthDto.ChangePasswordDto request)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId is null)
        {
            return Unauthorized();
        }

        await _authService.ChangePasswordAsync(userId, request);

        return Ok(new
        {
            Message = "Password has been changed successfully"
        });
    }


    [Authorize(Roles = "Patient")]
    [HttpGet("me")]
    public async Task<IActionResult> MyDetails()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        var userDetails = await _authService.GetDetailsAsync(userId);
        return Ok(userDetails);
    }

}
