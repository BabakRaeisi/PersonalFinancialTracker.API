using AutoMapper;
using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.Mappers
{
    public class TransactionToTransactionResponse : Profile
    {
        public TransactionToTransactionResponse() 
        {
            CreateMap<Transaction, TransactionResponse>()
                    .ForMember(dest => dest.TransactionID, opt => opt.MapFrom(src => src.TransationId))
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
