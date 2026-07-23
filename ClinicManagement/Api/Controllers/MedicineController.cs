using ApplicationCore.Medicine.Dto;
using ApplicationCore.Medicine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/medicines")]
public class MedicineController : ControllerBase
{
    private readonly IMedicineService _medicineService;

    public MedicineController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateMedicine(Guid id, [FromBody] MedicineDto.Request dto)
    {
        await _medicineService.UpdateMedicineAsync(id, dto);
        return NoContent();
    }


    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMedicine(Guid id)
    {
        await _medicineService.DeleteMedicineAsync(id);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetMedicines([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var data = await _medicineService.GetMedicinesAsync(pageNumber, pageSize);
        return Ok(data);
    }

}