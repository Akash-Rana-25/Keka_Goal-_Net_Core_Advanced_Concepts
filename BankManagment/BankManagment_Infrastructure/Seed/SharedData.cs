using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagment_Infrastructure.Seed
{
    public static class SharedData
    {
        public static List<Guid> BankAccountIds { get; set; } = new List<Guid>();
        public static List<Guid> PaymentMethodIds { get; set; } = new List<Guid>();
        public static List<Guid> AccountTypeIds { get; set; } = new List<Guid>();
    }
}
