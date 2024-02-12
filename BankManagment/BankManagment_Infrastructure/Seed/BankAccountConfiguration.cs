using BankManagment_Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BankManagment_Infrastructure.Seed
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        private readonly IConfiguration _configuration;

        public BankAccountConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            var numberOfBankAccountRecords = _configuration["AppSettings:NumberOfBankAccountRecords"];


            if (int.TryParse(numberOfBankAccountRecords, out int numberOfBankAccountRecordsInt))
            {
                var accountTypeIds = SharedData.AccountTypeIds;
                var random = new Random();
                var bankAccounts = Enumerable.Range(0, numberOfBankAccountRecordsInt)
                    .Select(i => new BankAccount
                    {
                        Id = Guid.NewGuid(),
                        FirstName = "Akash",
                        LastName = "Rana",
                        AccountNumber = GenerateRandomAccountNumber(),
                        OpeningDate = DateTime.Now.AddDays(-i),
                        AccountTypeId = GetRandomAccountTypeId(accountTypeIds, random)
                    })
                    .ToArray();

                builder.HasData(bankAccounts);
                SharedData.BankAccountIds = bankAccounts.Select(b => b.Id).ToList();
            }
        }
        private Guid GetRandomAccountTypeId(List<Guid> accountTypeIds, Random random)
        {
            return accountTypeIds[random.Next(accountTypeIds.Count)];
        }
        private string GenerateRandomAccountNumber()
        {
            var random = new Random();
            return random.Next(10000000, 99999999).ToString();
        }
    }

}