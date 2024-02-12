using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Context;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace BankManagment_Infrastructure.Seed
{
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        private readonly IConfiguration _configuration;

        public BankTransactionConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            var bankAccountIds = SharedData.BankAccountIds;
            var paymentMethodIds = SharedData.PaymentMethodIds;
            var numberOfBankTransactionRecords = _configuration["AppSettings:NumberOfBankTransactionRecords"];

            if (int.TryParse(numberOfBankTransactionRecords, out int numberOfBankTransactionRecordsInt) && numberOfBankTransactionRecordsInt > 0)
            {
                var random = new Random();

                var bankTransactions = Enumerable.Range(0, numberOfBankTransactionRecordsInt)
                    .Select(i => new BankTransaction
                    {
                        Id = Guid.NewGuid(),
                        TransactionPersonFirstName = "Akash",
                        TransactionPersonLastName = "Rana",
                        TransactionType = (i % 2 == 0) ? "Credit" : "Debit",
                        Category = GetRandomCategory(random),
                        Amount = (decimal)random.NextDouble() * 1000,
                        TransactionDate = DateTime.Now.AddDays(-i),
                        PaymentMethodID = GetRandomPaymentMethodId(paymentMethodIds, random),
                        BankAccountID = GetRandomBankAccountId(bankAccountIds, random)
                    })
                    .ToArray();

                builder.HasData(bankTransactions);
            }
        }

        private string GetRandomCategory(Random random)
        {
            var categories = new[] { "Opening Balance", "Bank Interest", "Bank Charges", "Normal Transactions" };
            return categories[random.Next(categories.Length)];
        }
        private Guid GetRandomBankAccountId(List<Guid> bankAccountIds, Random random)
        {
            return bankAccountIds[random.Next(bankAccountIds.Count)];
        }
        private Guid GetRandomPaymentMethodId(List<Guid> paymentMethodIds, Random random)
        {
            return paymentMethodIds[random.Next(paymentMethodIds.Count)];
        }
    }
}