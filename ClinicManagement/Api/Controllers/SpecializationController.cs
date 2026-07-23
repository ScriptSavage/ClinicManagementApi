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


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewSpecialization(SpecializationDto.AddNewSpecialization specializationDto)
    {
        await _specializationService.AddNewSpecialization(specializationDto);
        return Ok(new
        {
            Message = "Specialization added successfully",
        });
    }
    
}