using AutoMapper;
using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;


namespace PersonalFinancialTracker.Core.Mappers
{
    public class TransactionToTransactionResponse : Profile
    {
        public TransactionToTransactionResponse() 
        {
            CreateMap<Transaction, TransactionResponse>()
                    .ForMember(dest => dest.TransactionID, opt => opt.MapFrom(src => src.TransactionId))
                    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId)) // Add this
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.Payor, opt => opt.MapFrom(src => src.payor))
                    .ForMember(dest => dest.Payee, opt => opt.MapFrom(src => src.payee))
                    .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? string.Empty))
                    .ForMember(dest => dest.TypeOfTransaction, opt => opt.MapFrom(src => src.TypeOfTransaction))
                    .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.Created));


        }
    }
}
