using AutoMapper;
using BankManagment_Domain.Entity;
using BankManagment_DTO;
using Microsoft.AspNetCore.Mvc;
using BankManagment_Services;
using System.Web.Http;
using Serilog;

[ApiVersion("1.0")]
[RoutePrefix("api/v{version:apiVersion}/paymentmethods")]
[ResponseCache(Duration = 60)]
public class PaymentMethodController : Controller
{
    private readonly IPaymentMethodService _paymentMethodService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public PaymentMethodController(IPaymentMethodService paymentMethodService, IMapper mapper)
    {
        _paymentMethodService = paymentMethodService;
        _mapper = mapper;
        _logger = Log.ForContext<PaymentMethodController>();

    }

    [Microsoft.AspNetCore.Mvc.HttpGet("paymentmethods")]
    public async Task<IActionResult> GetPaymentMethods()
    {
        _logger.Information("Fetching All Payment Methods ");
        var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
        var paymentMethodDTOs = _mapper.Map<List<PaymentMethodDTO>>(paymentMethods);
        return Ok(paymentMethodDTOs);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Microsoft.AspNetCore.Mvc.HttpPost("paymentmethods")]
    public async Task<IActionResult> CreatePaymentMethod([Microsoft.AspNetCore.Mvc.FromBody] PaymentMethodDTO paymentMethodDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDTO);
        _logger.Information("Creating Payment Methods ");
        await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod);
        await _paymentMethodService.SaveChangesAsync();

        var createdDTO = _mapper.Map<PaymentMethodDTO>(paymentMethod);
        return CreatedAtAction(nameof(GetPaymentMethods), new { id = createdDTO.Id }, createdDTO);
    }

    [Microsoft.AspNetCore.Mvc.HttpPut("paymentmethods/{id}")]
    public async Task<IActionResult> UpdatePaymentMethod(Guid id, [Microsoft.AspNetCore.Mvc.FromBody] PaymentMethodDTO updatedPaymentMethodDTO)
    {
        if (!ModelState.IsValid || id != updatedPaymentMethodDTO.Id)
        {
            _logger.Error($"Model Invalid Or ID mismatch {id}");
            return BadRequest();
        }
            

        var updatedPaymentMethod = _mapper.Map<PaymentMethod>(updatedPaymentMethodDTO);
        _logger.Information($"Payment method Updated for Id {id}");
        await _paymentMethodService.UpdatePaymentMethodAsync(id, updatedPaymentMethod);
        await _paymentMethodService.SaveChangesAsync();

        return NoContent();
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete("paymentmethods/{id}")]
    public async Task<IActionResult> DeletePaymentMethod(Guid id)
    {
        _logger.Information($"Payment Method Deleted for Id {id}");
        await _paymentMethodService.DeletePaymentMethodAsync(id);
        await _paymentMethodService.SaveChangesAsync();

        return NoContent();
    }
}
