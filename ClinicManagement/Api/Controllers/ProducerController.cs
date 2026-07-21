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

    public ProducerController(IProducerService producerService)
    {
        _producerService = producerService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Post([FromBody] ProducerDto.NewProducer producerDto)
    {
        await _producerService.AddNewProducer(producerDto);
        return Ok(new
        {
            Message = "Producer created"
        });
    }
}