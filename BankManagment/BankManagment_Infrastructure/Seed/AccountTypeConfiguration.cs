using BankManagment_Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace BankManagment_Infrastructure.Seed
{
    public class AccountTypeConfiguration : IEntityTypeConfiguration<AccountType>
    {

        public void Configure(EntityTypeBuilder<AccountType> builder)
        {
            var liabilityId = Guid.NewGuid();
            var assetId = Guid.NewGuid();

            builder.HasData(
                new AccountType { Id = liabilityId, Name = "Liability" },
                new AccountType { Id = assetId, Name = "Asset" }
            );

            SharedData.AccountTypeIds = new List<Guid> { liabilityId, assetId };
        }

    }
}