using System.Security.Claims;
using ApplicationCore.Visit.Dto;
using ApplicationCore.Visit.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[Route("api/visits")]
public class VisitController : ControllerBase
{
    private readonly IVisitService _visitService;
    
    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }


    [HttpPost]
    [Authorize(Roles =  "Patient")]
    public async Task<IActionResult> CreateNewVisit([FromBody] VisitDto.CreateVisitRequest visitDto)
    {
        var user = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (user is null)
        {
            return Unauthorized();
        }

        var visit = await _visitService.CreateNewVisitAsync(user,visitDto);
       
        return Ok(new
        {
            Message = $"Visit has been created successfully at {visit.VisitDate}"
        });
    }

}