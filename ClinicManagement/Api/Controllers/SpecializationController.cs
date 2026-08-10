using ApplicationCore.Specialization.Dto;
using ApplicationCore.Specialization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/specializations")]
public class SpecializationController : ControllerBase
{
    private readonly ISpecializationService _specializationService;

    public SpecializationController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }


    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSpecializations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var specializations = await _specializationService
            .GetSpecializations(pageNumber, pageSize);
        
        return Ok(specializations);
    }

    [HttpGet("{specializationId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetSpecialization(Guid specializationId)
    {
        var specialization = await _specializationService.GetSpecialization(specializationId);
        return Ok(specialization);
    }

    [HttpGet("{specializationId:guid}/doctors")]
    public async Task<IActionResult> GetDoctorsBySpecialization(Guid specializationId, 
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var doctors = await _specializationService
            .GetDoctorsDetailsBySpecialization(specializationId, pageNumber, pageSize);
        
        return Ok(doctors);
       
    }

    [HttpPatch("{specializationId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ModifySpecialization(Guid specializationId,
        [FromBody] SpecializationDto.NewSpecialization request)
    {
        await _specializationService.ModifySpecialization(specializationId,request);
        return Ok(new
        {
            Message = "Specialization updated successfully",
        });
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewSpecialization(SpecializationDto.NewSpecialization specializationDto)
    {
        await _specializationService.AddNewSpecialization(specializationDto);
        return Ok(new
        {
            Message = "Specialization added successfully",
        });
    }
    
    
    
    
}