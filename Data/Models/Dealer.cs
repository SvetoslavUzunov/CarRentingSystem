﻿namespace CarRentingSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Data.DataConstants.Dealer;

public class Dealer
{
    public int Id { get; init; }

    [Required]
    [MaxLength(NameMaxLength)]
    public string Name { get; set; }

    [Required]
    [MaxLength(PhoneNumberMaxLength)]
    public string PhoneNumber { get; set; }

    [Required]
    public string UserId { get; set; }

    public IEnumerable<Car> Cars { get; init; } = new List<Car>();
}