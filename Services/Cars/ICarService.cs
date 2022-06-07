namespace CarRentingSystem.Services.Cars;

using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Cars.Models;

public interface ICarService
{
    CarQueryServiceModel All(string brand, string searchTerm, CarSorting sorting, int currentPage, int carsPerPage);

    IEnumerable<CarServiceModel> ByUser(string userId);

    IEnumerable<string> AllBrands();

    IEnumerable<CarCategoryServiceModel> AllCategories();

    CarDetailsServiceModel Details(int carId);

    bool CategoryExist(int categoryId);

    int Create(
           string brand,
           string model,
           string description,
           string imageUrl,
           int year,
           int categoryId,
           int dealerId);

    bool Edit(
           int carId,
           string brand,
           string model,
           string description,
           string imageUrl,
           int year,
           int categoryId);

    bool IsByDealer(int carId, int dealerId);

    void Delete(int id);
}