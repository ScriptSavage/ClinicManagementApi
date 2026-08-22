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

    [HttpGet("{visitId:guid}")]
    [Authorize(Roles = "Admin, Doctor")]
    public async Task<IActionResult> GetVisitDetails(Guid visitId)
    {
        var visitDetails = await _visitService.GetVisitDetailsAsync(visitId);
        return Ok(visitDetails);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Doctor")]
    public async Task<IActionResult> GetAllVisits([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var visits = await _visitService.GetVisitsDetailsAsync(pageNumber, pageSize);
        return Ok(visits);
    }


    [HttpGet("me")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetMyVisits()
    {
        var user = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (user is null)
        {
            return Unauthorized();
        }
        
        var myVisits = await _visitService.GetMyVisitsAsync(user);
        
        return Ok(myVisits);
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
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

    [HttpPut("{visitId:guid}/completion")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> CompleteVisit(Guid visitId, [FromBody] VisitDto.CreateDescription dto)
    {
        var user = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (user is null)
        {
            return Unauthorized();
        }

        await _visitService.CompleteVisitAsync(visitId, dto);
        
        return Ok(new
        {
            Message = "Visit Completed"
        });
    }
    
    

}