using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WhitePayment.Application.Dto;
using WhitePayment.Application.Interfaces;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/paystack")]
public class PaymentController(IPaymentService service, IWebhookHandlerService webhookHandlerService) : ControllerBase
{
    [HttpPost("initialize")]
    public async Task<IActionResult> Initialize([FromBody] InitializePayment dto)
    {
        var result = await service.InitializeAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("verify/{reference}")]
    public async Task<IActionResult> Verify([FromRoute] string reference)
    {
        var result = await service.VerifyAsync(reference);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Handle()
    {
        using var reader = new StreamReader(Request.Body);
        var payload = await reader.ReadToEndAsync();

        if (!Request.Headers.TryGetValue("x-paystack-signature", out var signatureHeader))
            return BadRequest();

        var result = await webhookHandlerService
            .HandleWebhookAsync(payload, signatureHeader);

        return StatusCode(result.StatusCode, result);
    }

}
