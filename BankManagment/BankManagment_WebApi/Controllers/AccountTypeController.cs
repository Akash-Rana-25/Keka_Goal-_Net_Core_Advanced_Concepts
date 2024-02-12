using AutoMapper;
using BankManagment_Domain.Entity;
using BankManagment_DTO;
using Microsoft.AspNetCore.Mvc;
using BankManagment_Services;
using System.Web.Http;
using Serilog;

[ApiVersion("1.0")]
[RoutePrefix("api/v{version:apiVersion}/accounttypes")]
[ResponseCache(Duration = 60)] 
public class AccountTypeController : Controller
{
    private readonly IAccountTypeService _accountTypeService;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;

    public AccountTypeController(IAccountTypeService accountTypeService, IMapper mapper)
    {
        _accountTypeService = accountTypeService;
        _mapper = mapper;
        _logger = Log.ForContext<AccountTypeController>();
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("accounttypes")]
    public async Task<IActionResult> GetAccountTypes()
    {
        _logger.Information("Fetching all account types.");
        var accountTypes = await _accountTypeService.GetAllAccountTypesAsync();
        var accountTypeDTOs = _mapper.Map<List<AccountTypeDTO>>(accountTypes);
        return Ok(accountTypeDTOs);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [Microsoft.AspNetCore.Mvc.HttpPost("accounttypes")]
    public async Task<IActionResult> CreateAccountType([Microsoft.AspNetCore.Mvc.FromBody] AccountTypeDTO accountTypeDTO)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var accountType = _mapper.Map<AccountType>(accountTypeDTO);
        _logger.Information("Creating a new account type.");
        await _accountTypeService.CreateAccountTypeAsync(accountType);
        await _accountTypeService.SaveChangesAsync();

        var createdDTO = _mapper.Map<AccountTypeDTO>(accountType);
        return CreatedAtAction(nameof(GetAccountTypes), new { id = createdDTO.Id }, createdDTO);
    }

    [Microsoft.AspNetCore.Mvc.HttpPut("accounttypes/{id}")]
    public async Task<IActionResult> UpdateAccountType(Guid id, [Microsoft.AspNetCore.Mvc.FromBody] AccountTypeDTO updatedAccountTypeDTO)
    {
        if (!ModelState.IsValid)
        {
            _logger.Information("Invalid model state while updating account type.");
            return BadRequest();
        }

        if (id != updatedAccountTypeDTO.Id)
        {
            _logger.Information("Mismatched ID in the request while updating account type.");
            return BadRequest();
        }
        _logger.Information($"Updating account type with ID {id}.");
        var updatedAccountType = _mapper.Map<AccountType>(updatedAccountTypeDTO);

        await _accountTypeService.UpdateAccountTypeAsync(id, updatedAccountType);
        await _accountTypeService.SaveChangesAsync();

        

        return NoContent();
    }


    [Microsoft.AspNetCore.Mvc.HttpDelete("accounttypes/{id}")]
    public async Task<IActionResult> DeleteAccountType(Guid id)
    {
        _logger.Information($"Deleting account type with ID {id}.");
        await _accountTypeService.DeleteAccountTypeAsync(id);
        await _accountTypeService.SaveChangesAsync();

        return NoContent();
    }
}
