using ApplicationCore.Patient.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
   private readonly IPatientService _service;

   public PatientController(IPatientService service)
   {
      _service = service;
   }


   [HttpGet]
   public async Task<IActionResult> GetPatientsData([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
   {
      var result = await _service.GetAllPatientsAsync(pageNumber, pageSize);
      return Ok(result);
   }
}