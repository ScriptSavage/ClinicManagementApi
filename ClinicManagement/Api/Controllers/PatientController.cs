using System.Security.Claims;
using ApplicationCore.Patient.Dto;
using ApplicationCore.Patient.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Api.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientController : ControllerBase
{
   private readonly IPatientService _patientService;

   public PatientController(IPatientService patientService)
   {
      _patientService = patientService;
   }

   
   [HttpGet]
   public async Task<IActionResult> GetPatients([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
   {
      var result = await _patientService.GetAllPatientsAsync(pageNumber, pageSize);
      return Ok(result);
   }

   [HttpGet("me/visits")]
   [Authorize(Roles = "Patient")]
   public async Task<IActionResult> GetMyVisits()
   {
      var patient = User.FindFirstValue(JwtRegisteredClaimNames.Sub);

      if (string.IsNullOrWhiteSpace(patient))
      {
         return Unauthorized();
      }

      var patientVisitsAsync = await _patientService.GetPatientVisitsAsync(patient);
      return Ok(patientVisitsAsync);
   }
   



   [HttpGet("me/prescriptions")]
   [Authorize(Roles = "Patient")]
   public async Task<IActionResult> GetMyPrescriptions()
   {
      var patient = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
      if (string.IsNullOrWhiteSpace(patient))
      {
         return Unauthorized();
      }
      
      var patientPrescriptions = await _patientService.GetPatientPrescriptionsAsync(patient);
      
      return Ok(patientPrescriptions);
   }


   [Authorize(Roles = "Admin")]
   [HttpGet("{patientId:guid}")]
   public async Task<IActionResult> GetPatientById(Guid patientId)
   {
      var  result = await _patientService.GetPatientByIdAsync(patientId);
      return Ok(result);
   }


   [HttpPatch("{patientId:guid}")]
   [Authorize(Roles = "Admin")]
   public async Task<IActionResult> UpdatePatient(Guid patientId, [FromBody] PatientDto.UpdatePatientRequest request)
   {
      await _patientService.UpdatePatientAsync(patientId, request);
      return Ok(new
      {
         Message = "Patient updated successfully"
      });
   }


   [HttpDelete("{patientId:guid}")]
   [Authorize(Roles = "Admin")]
   public async Task<IActionResult> DeletePatient(Guid patientId)
   {
      await _patientService.DeletePatientAsync(patientId);
      return Ok(new
         {
            Message = "Patient deleted successfully"
         });
   }


}