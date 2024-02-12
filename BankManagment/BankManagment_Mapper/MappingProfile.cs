using AutoMapper;
using BankManagment_Domain.Entity;
using BankManagment_DTO;

namespace BankManagment_Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BankAccount,BankAccountDTO >().ReverseMap();
            CreateMap<AccountType, AccountTypeDTO>().ReverseMap();
            CreateMap<BankTransaction,BankTransactionDTO >().ReverseMap();
            CreateMap<PaymentMethod,PaymentMethodDTO >().ReverseMap();

        }
    }
}
