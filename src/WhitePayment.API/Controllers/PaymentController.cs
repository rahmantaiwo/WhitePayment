using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using WhitePayment.Application.Common;
using WhitePayment.Application.Dto;
using WhitePayment.Application.Interfaces;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/paystack")]
public class PaymentController(IPaymentService service, IWebhookHandlerService webhookHandlerService) : ControllerBase
{
    /// <summary>
    /// Initialize a Paystack payment
    /// </summary>
    [HttpPost("initialize")]
    [ProducesResponseType(typeof(BaseResponseModel<InitializePaymentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseModel<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponseModel<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Initialize([FromBody] InitializePayment dto)
    {
        var result = await service.InitializeAsync(dto);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Verify a Paystack transaction by reference
    /// </summary>
    [HttpGet("verify/{reference}")]
    [ProducesResponseType(typeof(BaseResponseModel<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponseModel<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(BaseResponseModel<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Verify([FromRoute] string reference)
    {
        var result = await service.VerifyAsync(reference);
        return StatusCode(result.StatusCode, result);
    }

    /// <summary>
    /// Paystack webhook endpoint
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(BaseResponseModel<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(BaseResponseModel<object>), StatusCodes.Status500InternalServerError)]
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
