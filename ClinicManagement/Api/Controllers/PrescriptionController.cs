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

}