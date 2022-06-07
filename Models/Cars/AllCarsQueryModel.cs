namespace CarRentingSystem.Models.Cars;

using CarRentingSystem.Services.Cars.Models;
using System.ComponentModel.DataAnnotations;

public class AllCarsQueryModel
{
    public const int CarsPerPage = 3;

    public string Brand { get; init; }

    public IEnumerable<string> Brands { get; set; }

    [Display(Name = "Search by text")]
    public string SearchTerm { get; init; }

    public int CurrentPage { get; init; } = 1;

    public int TotalCars { get; set; }

    public CarSorting Sorting { get; init; }

    public IEnumerable<CarServiceModel> Cars { get; set; }
}
