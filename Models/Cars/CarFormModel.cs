namespace CarRentingSystem.Models.Cars;

using CarRentingSystem.Services.Cars.Models;
using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Data.DataConstants.Car;


public class CarFormModel
{
    [Required]
    [StringLength(BrandMaxLength, MinimumLength = BrandMinLength)]
    public string Brand { get; init; }

    [Required]
    [StringLength(ModelMaxLength, MinimumLength = ModelMinLength)]
    public string Model { get; init; }

    [Required]
    [StringLength(10000, MinimumLength = DescriptionMinLength)]
    public string Description { get; init; }

    [Display(Name = "Image URL")]
    [Required]
    [Url]
    public string ImageUrl { get; init; }

    [Range(YearMinValue, YearMaxValue)]
    public int Year { get; init; }

    [Display(Name = "Category")]
    public int CategoryId { get; init; }
    public IEnumerable<CarCategoryServiceModel>? Category { get; set; }
}
