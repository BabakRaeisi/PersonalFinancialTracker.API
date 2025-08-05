using AutoMapper;
using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;


namespace PersonalFinancialTracker.Core.Mappers
{
    public class TransactionAddRequestToTransactionMappingProfile  : Profile
    {
        public TransactionAddRequestToTransactionMappingProfile()
        {
            CreateMap< TransactionAddRequest, Transaction>()
                .ForMember(dest => dest.TransationId, opt => opt.Ignore())
                .ForMember(dest => dest.Title , opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.payor, opt => opt.MapFrom(src => src.Payor))
                .ForMember(dest => dest.payee, opt => opt.MapFrom(src => src.Payee))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty));
        }


    }
}
