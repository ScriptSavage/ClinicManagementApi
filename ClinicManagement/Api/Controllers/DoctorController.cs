using System.Security.Claims;
using ApplicationCore.Doctor.Dto;
using ApplicationCore.Doctor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly ILogger<DoctorController> _logger;

    public DoctorController(IDoctorService doctorService,
        ILogger<DoctorController> logger)
    {
        _doctorService = doctorService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewDoctor([FromBody] DoctorDto.CreateDoctorDto request)
    {
        await _doctorService.AddNewDoctor(request);
        return Ok(new { Message = "Doctor Added Successfully" });
    }

    [HttpPost("{id:guid}/specializations/{specializationId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewSpecialization(Guid id, Guid specializationId)
    {
        await _doctorService.AddNewSpecializationToDoctor(id, specializationId);
        return Ok(new { Message = "New Spec Added Successfully" });
    }

    [HttpDelete("{id:guid}/specializations/{specializationId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSpecialization(Guid id, Guid specializationId)
    {
        await _doctorService.DeleteDoctorSpecialization(id, specializationId);
        return Ok(new { Message = "Doctor Spec Removed Successfully" });
    }

    [HttpGet]
    public async Task<IActionResult> GetDoctors([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var data = await _doctorService.GetAllDoctorsAsync(pageNumber, pageSize);
        _logger.LogInformation("Get all doctors");
        return Ok(data);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetMyDetails()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (userId is null)
        {
            return Unauthorized();
        }

        var doctorDetails = await _doctorService.GetMyDetails(userId);
        return Ok(doctorDetails);
    }

    [HttpGet("me/vists")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetMyVisits()
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (userId is null)
        {
            return Unauthorized();
        }
        
        var doctorVisitsDetails = await _doctorService.GetDoctorVisitsByUserAsync(userId);
        return Ok(doctorVisitsDetails);
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        return Ok(doctor);
    }
    
    [HttpGet("{id:guid}/specializations")]
    public async Task<IActionResult> GetDoctorSpecializationsById(Guid id)
    {
        var doctorSpecializations = await _doctorService.GetDoctorSpecializationById(id);
        return Ok(doctorSpecializations);
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateDoctor(Guid id, [FromBody] DoctorDto.UpdateDoctorDto request)
    {
        await _doctorService.UpdateDoctor(id, request);
        return Ok(new { Message = "Doctor Updated Successfully" });
    }

}