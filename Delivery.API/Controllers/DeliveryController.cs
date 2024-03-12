using Delivery.Application.Interfaces.Customers;
using Delivery.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Delivery.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeliveryController(ISendEmailUseCase sendEmailUseCase) : ControllerBase
{
    private readonly ISendEmailUseCase _sendEmailUseCase = sendEmailUseCase;

    [HttpPost("send-email")]
    public IActionResult SendEmail([FromBody] SendEmailDTO dto)
    {
        var result = _sendEmailUseCase.Execute(dto);
        if (result == null) return BadRequest("Error to send email.");
        return NoContent();
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        return Ok(DateTime.Now);
    }
}
