using AutoMapper;
using TS.Domain.DTOs;
using TS.Domain.Entities;

namespace TS.API.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Transaction, TransactionDto>()
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.RecurrentTransaction, opt => opt.MapFrom(src => src.RecurrentTransaction))
            .ForMember(dest => dest.TransactionType, opt => opt.MapFrom(src => src.TransactionType));
        
        CreateMap<Currency, CurrencyDto>();
        
        CreateMap<RecurrentTransaction, RecurrentTransactionDto>();
        
        CreateMap<Transactiontype, TransactionTypeDto>();
        
        CreateMap<Account, AccountDto>();

    }
}