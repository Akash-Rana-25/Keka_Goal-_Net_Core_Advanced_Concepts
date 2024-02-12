using AutoMapper;
using BankManagment_Domain.Entity;
using BankManagment_DTO;
using BankManagment_Services;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using Serilog;

[ApiVersion("1.0")]
[RoutePrefix("api/v{version:apiVersion}/bankaccounts")]
[ResponseCache(Duration = 60)] 
public class BankAccountController : Controller
{
    private readonly IBankAccountService _bankAccountService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public BankAccountController(IBankAccountService bankAccountService, IMapper mapper)
    {
        _bankAccountService = bankAccountService;
        _mapper = mapper;
        _logger = Log.ForContext<BankAccountController>();
    }
    [Microsoft.AspNetCore.Mvc.HttpGet("bankaccounts")]
    public async Task<IActionResult> GetBankAccounts()
    {
        _logger.Information("Fetching all Bank Account .");
        var bankAccounts = await _bankAccountService.GetAllBankAccountsAsync();
        var bankAccountDTOs = _mapper.Map<List<BankAccountDTO>>(bankAccounts);
        return Ok(bankAccountDTOs);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Microsoft.AspNetCore.Mvc.HttpPost("bankaccounts")]
    public async Task<IActionResult> CreateBankAccount([Microsoft.AspNetCore.Mvc.FromBody] BankAccountDTO bankAccountDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var bankAccount = _mapper.Map<BankAccount>(bankAccountDTO);
        _logger.Information("Creating a new  Bank Account");
        await _bankAccountService.CreateBankAccountAsync(bankAccount);
        await _bankAccountService.SaveChangesAsync();

        var createdDTO = _mapper.Map<BankAccountDTO>(bankAccount);
        return CreatedAtAction(nameof(GetBankAccounts), new { id = createdDTO.Id }, createdDTO);
    }

    [Microsoft.AspNetCore.Mvc.HttpPut("bankaccounts/{id}")]
    public async Task<IActionResult> UpdateBankAccount(Guid id, [Microsoft.AspNetCore.Mvc.FromBody] BankAccountDTO updatedBankAccountDTO)
    {
        if (!ModelState.IsValid || id != updatedBankAccountDTO.Id)
        {
            _logger.Information("Invalid model state while updating Bank Account.");
            return BadRequest();
        }

        var updatedBankAccount = _mapper.Map<BankAccount>(updatedBankAccountDTO);
        _logger.Information($"Updating Bank Account with ID {id}.");
        await _bankAccountService.UpdateBankAccountAsync(id, updatedBankAccount);
        await _bankAccountService.SaveChangesAsync();

        return NoContent();
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete("bankaccounts/{id}")]
    public async Task<IActionResult> DeleteBankAccount(Guid id)
    {
        _logger.Information($"Deleting Bank Account with ID {id}.");
        await _bankAccountService.DeleteBankAccountAsync(id);
        await _bankAccountService.SaveChangesAsync();

        return NoContent();
    }
}


