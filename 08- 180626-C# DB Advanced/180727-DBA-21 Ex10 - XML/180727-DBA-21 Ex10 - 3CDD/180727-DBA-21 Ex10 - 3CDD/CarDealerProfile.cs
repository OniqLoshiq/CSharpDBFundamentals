using _180727_DBA_21_Ex10___3CDD.Dtos;
using AutoMapper;
using CarDealer.Models;
using System.Linq;

namespace _180727_DBA_21_Ex10___3CDD
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            CreateMap<SupplierDto, Supplier>();
            CreateMap<PartDto, Part>();
            CreateMap<CarDto, Car>();
            CreateMap<CustomerDto, Customer>();

            CreateMap<Customer, Q5_CustomerDto>()
                .ForMember(cfg => cfg.BoughtCars,
                            opt => opt.MapFrom(c => c.Sales.Count))
                .ForMember(cfg => cfg.DiscountsAndCarPrices,
                            opt => opt.MapFrom(s => s.Sales.Select(x => new Q5_DiscountAndCarPriceDto
                            {
                                Discount = x.Discount,
                                TotalCarPrice = x.Car.PartCars.Select(p => p.Part.Price).Sum()
                            })));

            CreateMap<Sale, Q6_SaleDto>()
                .ForMember(cfg => cfg.CustomerName,
                            opt => opt.MapFrom(s => s.Customer.Name))
                .ForMember(cfg => cfg.CarData,
                            opt => opt.MapFrom(x => x.Car))
                .ForMember(cfg => cfg.CarPrice,
                            opt => opt.MapFrom(x => x.Car.PartCars.Select(p => p.Part.Price).Sum()));
        }
    }
}
