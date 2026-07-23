using ApplicationCore.Medicine.Dto;
using ApplicationCore.Medicine.Services;
using ApplicationCore.Producer.Dto;
using ApplicationCore.Producer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/producers")]
public class ProducerController : ControllerBase
{
    private readonly IProducerService _producerService;
    private readonly IMedicineService _medicineService;

    public ProducerController(IProducerService producerService,
        IMedicineService medicineService)
    {
        _producerService = producerService;
        _medicineService = medicineService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewProducer([FromBody] ProducerDto.NewProducer producerDto)
    {
        await _producerService.AddNewProducer(producerDto);
        return Ok(new
        {
            Message = "Producer created"
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProducer(Guid id)
    {
        await _producerService.DeleteProducer(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/medicines")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddNewMedicine(Guid id, [FromBody] MedicineDto.Request dto)
    {
        await _medicineService.AddNewMedicineAsync(id, dto);
        return Ok(new
        {
            Message = "Medicine added"
        });
    }

}