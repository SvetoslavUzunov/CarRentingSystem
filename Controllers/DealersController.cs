namespace CarRentingSystem.Controllers;

using CarRentingSystem.Data;
using CarRentingSystem.Data.Models;
using CarRentingSystem.Infrastructure.Extensions;
using CarRentingSystem.Models.Dealers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static WebConstants;

public class DealersController : Controller
{
    private readonly CarRentingDbContext data;

    public DealersController(CarRentingDbContext data)
        => this.data = data;

    [Authorize]
    public IActionResult Become() => View();

    [Authorize]
    [HttpPost]
    public IActionResult Become(BecomeADealerFormModel dealer)
    {
        var userId = User.Id();

        var userIsAlreadyDealer = data.Dealers
            .Any(d => d.UserId == userId);

        if (userIsAlreadyDealer)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(dealer);
        }

        var dealerData = new Dealer
        {
            Name = dealer.Name,
            PhoneNumber = dealer.PhoneNumber,
            UserId = userId
        };

        data.Dealers.Add(dealerData);
        data.SaveChanges();

        TempData[GlobalMessageKey] = "Thank you for becoming a dealer!";

        return RedirectToAction("All", "Cars");
    }
}