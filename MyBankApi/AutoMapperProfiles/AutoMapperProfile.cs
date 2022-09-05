using AutoMapper;
using MyBankApi.DTOs;
using MyBankApi.Models;

namespace MyBankApi.AutoMapperProfiles
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AccountUpdateDto, Account>()
                .ReverseMap();

            CreateMap<NewAccountDto, Account>()
                .ReverseMap();

            CreateMap<Account, AccountDto>();
        }
    }
}
