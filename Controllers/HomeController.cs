namespace CarRentingSystem.Controllers;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarRentingSystem.Data;
using CarRentingSystem.Models;
using CarRentingSystem.Models.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : Controller
{
    private readonly CarRentingDbContext data;
    private readonly IConfigurationProvider mapper;

    public HomeController(IMapper mapper, CarRentingDbContext data)
    {
        this.mapper = mapper.ConfigurationProvider;
        this.data = data;
    }

    public IActionResult Index()
    {
        var cars = data.Cars
           .OrderByDescending(x => x.Id)
           .ProjectTo<CarIndexViewModel>(this.mapper)
           .Take(3)
           .ToList();

        var totalCars = data.Cars.Count();
        var totalUsers = data.Users.Count();

        return View(new IndexViewModel
        {
            TotalCars = totalCars,
            TotalUsers = totalUsers,
            Cars = cars,
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}