using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationCore.Prescription.Dto;
using ApplicationCore.Prescription.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionController : ControllerBase
{
    private readonly IPrescriptionService _prescriptionService;

    public PrescriptionController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }


    [HttpPost("{patientId:guid}")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> AddNewPrescription(Guid patientId, [FromBody] PrescriptionDto.Request dto)
    {
        var user = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (user is null)
        {
            return Unauthorized();
        }
        
        await _prescriptionService.AddNewPrescription(patientId, user, dto);
        
        return StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet("{prescriptionId:guid}")]
    [Authorize(Roles = "Admin,Doctor,Patient")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPrescription(Guid prescriptionId)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        var result = await _prescriptionService
            .GetPrescriptionByIdAsync(userId, prescriptionId);

        return Ok(result);
    }

}