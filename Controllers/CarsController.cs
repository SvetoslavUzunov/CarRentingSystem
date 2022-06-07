namespace CarRentingSystem.Controllers;

using AutoMapper;
using CarRentingSystem.Infrastructure.Extensions;
using CarRentingSystem.Models.Cars;
using CarRentingSystem.Services.Cars;
using CarRentingSystem.Services.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebConstants;

public class CarsController : Controller
{
    private readonly ICarService cars;
    private readonly IDealerService dealers;
    private readonly IMapper mapper;

    public CarsController(ICarService cars, IDealerService dealers, IMapper mapper)
    {
        this.cars = cars;
        this.dealers = dealers;
        this.mapper = mapper;
    }

    [Authorize]
    public IActionResult Add()
    {
        if (!dealers.IsDealer(User.Id()))
        {
            return RedirectToAction(nameof(DealersController.Become), "Dealers");
        }

        return View(new CarFormModel
        {
            Category = this.cars.AllCategories()
        });
    }

    [HttpPost]
    [Authorize]
    public IActionResult Add(CarFormModel car)
    {
        var dealerId = dealers.IdByUser(User.Id());

        if (dealerId == 0)
        {
            return RedirectToAction(nameof(DealersController.Become), "Dealers");
        }

        if (!cars.CategoryExist(car.CategoryId))
        {
            ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            car.Category = this.cars.AllCategories();

            return View(car);
        }

        cars.Create(car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.CategoryId, dealerId);

        TempData[GlobalMessageKey] = "Car was added!";

        return RedirectToAction(nameof(All));
    }

    public IActionResult All([FromQuery] AllCarsQueryModel query)
    {
        var queryResult = this.cars.All(query.Brand, query.SearchTerm, query.Sorting, query.CurrentPage, AllCarsQueryModel.CarsPerPage);

        var carBrands = this.cars.AllBrands();

        query.Brands = carBrands;
        query.TotalCars = queryResult.TotalCars;
        query.Cars = queryResult.Cars;

        return View(query);
    }

    [Authorize]
    public IActionResult Mine()
    {
        var myCars = cars.ByUser(User.Id());

        return View(myCars);
    }

    [Authorize]
    public IActionResult Edit(int id)
    {
        var userId = User.Id();

        if (!dealers.IsDealer(userId))
        {
            return RedirectToAction(nameof(DealersController.Become), "Dealers");
        }

        var car = cars.Details(id);
        if (car.UserId != userId)
        {
            return Unauthorized();
        }

        var carForm = mapper.Map<CarFormModel>(car);
        carForm.Category = cars.AllCategories();

        return View(carForm);
    }

    [Authorize]
    [HttpPost]
    public IActionResult Edit(int id, CarFormModel car)
    {
        var dealerId = dealers.IdByUser(User.Id());

        if (dealerId == 0)
        {
            return RedirectToAction(nameof(DealersController.Become), "Dealers");
        }

        if (!cars.CategoryExist(car.CategoryId))
        {
            ModelState.AddModelError(nameof(car.CategoryId), "Category does not exist.");
        }

        if (!ModelState.IsValid)
        {
            car.Category = this.cars.AllCategories();

            return View(car);
        }

        if (!cars.IsByDealer(id, dealerId))
        {
            return BadRequest();
        }

        cars.Edit(id, car.Brand, car.Model, car.Description, car.ImageUrl, car.Year, car.CategoryId);

        TempData[GlobalMessageKey] = "Car was edited!";

        return RedirectToAction(nameof(All));
    }

    public IActionResult Delete(int id)
    {
        cars.Delete(id);

        TempData[GlobalMessageKey] = "Car was deleted!";

        return RedirectToAction(nameof(Mine));
    }

    public IActionResult Details(int id)
    {
        var car = cars.Details(id);

        return View(car);
    }
}