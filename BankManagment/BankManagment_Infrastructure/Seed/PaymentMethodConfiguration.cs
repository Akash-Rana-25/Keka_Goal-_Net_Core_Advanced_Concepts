using BankManagment_Domain.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BankManagment_Infrastructure.Seed
{
    public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            var cashId = Guid.NewGuid();
            var chequeId = Guid.NewGuid();
            var neftId = Guid.NewGuid();
            var rtgsId = Guid.NewGuid();
            var otherId = Guid.NewGuid();

            builder.HasData(
                new PaymentMethod { Id = cashId, Name = "Cash" },
                new PaymentMethod { Id = chequeId, Name = "Cheque" },
                new PaymentMethod { Id = neftId, Name = "NEFT" },
                new PaymentMethod { Id = rtgsId, Name = "RTGS" },
                new PaymentMethod { Id = otherId, Name = "Other" }
            );

            SharedData.PaymentMethodIds = new List<Guid> { cashId, chequeId, neftId, rtgsId, otherId };
        }

    }
}