namespace CarRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Data.DataConstants.Category;

public class Category
{
    public int Id { get; init; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; }

    public virtual IEnumerable<Car> Cars { get; init; } = new List<Car>();
}
