namespace CarRentingSystem.Infrastructure;

using AutoMapper;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Models.Home;
using CarRentingSystem.Services.Cars.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CarDetailsServiceModel, CarFormModel>();
        CreateMap<Car, CarIndexViewModel>();
        CreateMap<Car, CarDetailsServiceModel>()
            .ForMember(x => x.UserId, cfg => cfg.MapFrom(x => x.Dealer.UserId))
            .ForMember(x => x.CategoryName, cfg => cfg.MapFrom(x => x.Category.Name));
        CreateMap<Category, CarCategoryServiceModel>();
        CreateMap<Car, CarServiceModel>()
            .ForMember(x => x.CategoryName, cfg => cfg.MapFrom(x => x.Category.Name));
    }
}
