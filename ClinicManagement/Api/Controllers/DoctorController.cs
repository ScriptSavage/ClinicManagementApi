using ApplicationCore.Doctor.Dto;
using ApplicationCore.Doctor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromBody] DoctorDto.CreateDoctorDto request)
    {
        await _doctorService.AddNewDoctor(request);
        return Ok(new { Message = "Doctor Added Successfully" });
    }
}