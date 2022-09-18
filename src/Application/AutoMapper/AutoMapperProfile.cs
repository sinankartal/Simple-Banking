using AutoMapper;
using Common;
using Common.DTOs;
using Persistence.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AccountHolder, AccountHolderDTO>();

        CreateMap<Account, AccountDTO>()
            .ForPath(dest => dest.AccountHolder, src => src.MapFrom(x => x.Holder));

    }
}