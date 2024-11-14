using AutoMapper;
using WarehouseWebsite.Application.Models;
using WarehouseWebsite.Domain.Models.Orders;

namespace WarehouseWebsite.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Order, OrderDTO>()
                .ForMember(oDTO => oDTO.Id, o => o.MapFrom(src => src.Id))
                .ForMember(oDTO => oDTO.OrderTime, o => o.MapFrom(src => src.OrderTime))
                .ForMember(oDTO => oDTO.Status, o => o.MapFrom(src => src.Status))
                .ForMember(oDTO => oDTO.TotalPrice, o => o.MapFrom(src => src.TotalPrice))
                .ForMember(oDTO => oDTO.OrderItems, o => o.MapFrom(src => src.OrderItems))
                .ReverseMap()
                .ForSourceMember(src => src.Id, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.OrderTime, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Status, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.TotalPrice, opt => opt.DoNotValidate())
                .ForMember(o => o.OrderItems, oDTO => oDTO.MapFrom(src => src.OrderItems));
        }
    }
}
