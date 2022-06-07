namespace CarRentingSystem.Services.Cars;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Cars.Models;

public class CarService : ICarService
{
    private readonly CarRentingDbContext data;
    private readonly IConfigurationProvider mapper;

    public CarService(CarRentingDbContext data, IMapper mapper)
    {
        this.data = data;
        this.mapper = mapper.ConfigurationProvider;
    }

    public CarQueryServiceModel All(string brand, string searchTerm, CarSorting sorting, int currentPage, int carsPerPage)
    {
        var carsQuery = data.Cars.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
        {
            carsQuery = carsQuery
                .Where(c => c.Brand == brand);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            carsQuery = carsQuery
                .Where(c => c.Description.ToLower().Contains(searchTerm.ToLower()) ||
                            (c.Brand.ToLower() + " " + c.Model.ToLower()).Contains(searchTerm.ToLower()));
        }

        carsQuery = sorting switch
        {
            CarSorting.Year => carsQuery.OrderByDescending(c => c.Year),
            CarSorting.BrandAndModel => carsQuery.OrderBy(c => c.Brand).ThenBy(c => c.Model),
            CarSorting.DateCreated or _ => carsQuery.OrderByDescending(c => c.Id)
        };

        var totalCars = carsQuery.Count();

        var cars = GetCars(carsQuery
            .Skip((currentPage - 1) * carsPerPage)
            .Take(carsPerPage));

        return new CarQueryServiceModel
        {
            TotalCars = totalCars,
            CarsPerPage = carsPerPage,
            CurrentPage = currentPage,
            Cars = cars,
        };
    }

    public IEnumerable<string> AllBrands() => data.Cars
            .Select(c => c.Brand)
            .Distinct()
            .OrderBy(br => br)
            .ToList();

    public IEnumerable<CarServiceModel> ByUser(string userId)
        => GetCars(data.Cars.Where(c => c.Dealer.UserId == userId));


    public IEnumerable<CarCategoryServiceModel> AllCategories()
        => this.data
        .Category
        .ProjectTo<CarCategoryServiceModel>(mapper)
        .ToList();

    public CarDetailsServiceModel Details(int id)
        => data
        .Cars
        .Where(c => c.Id == id)
        .ProjectTo<CarDetailsServiceModel>(mapper)
        .FirstOrDefault();

    public bool CategoryExist(int categoryId)
        => data.Category.Any(x => x.Id == categoryId);

    public int Create(string brand, string model, string description, string imageUrl, int year, int categoryId, int dealerId)
    {
        var carData = new Car
        {
            Brand = brand,
            Model = model,
            Description = description,
            ImageUrl = imageUrl,
            Year = year,
            CategoryId = categoryId,
            DealerId = dealerId
        };

        data.Cars.Add(carData);
        data.SaveChanges();

        return carData.Id;
    }

    public bool Edit(int id, string brand, string model, string description, string imageUrl, int year, int categoryId)
    {
        var carData = data.Cars.Find(id);

        if (carData == null)
        {
            return false;
        }

        carData.Brand = brand;
        carData.Model = model;
        carData.Description = description;
        carData.ImageUrl = imageUrl;
        carData.Year = year;
        carData.CategoryId = categoryId;

        data.SaveChanges();

        return true;
    }

    public void Delete(int id)
    {
        var carToRemove = data.Cars.Where(c => c.Id == id).FirstOrDefault();

        if (carToRemove != null)
        {
            data.Cars.Remove(carToRemove);
            data.SaveChanges();
        }
    }

    public bool IsByDealer(int carId, int dealerId)
        => data.Cars.
        Any(c => c.Id == carId && c.DealerId == dealerId);

    private IEnumerable<CarServiceModel> GetCars(IQueryable<Car> carQuery)
        => carQuery
        .ProjectTo<CarServiceModel>(mapper)
        .ToList();
}