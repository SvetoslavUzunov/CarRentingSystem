namespace CarRentingSystem.Models.Home;

public class IndexViewModel
{
    public int TotalCars { get; init; }

    public int TotalUsers { get; init; }

    public int TotalRent { get; init; } 

    public List<CarIndexViewModel> Cars { get; init; }
}