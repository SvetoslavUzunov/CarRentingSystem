namespace CarRentingSystem.Models.Dealers;

using System.ComponentModel.DataAnnotations;
using static CarRentingSystem.Data.DataConstants.Dealer;

public class BecomeADealerFormModel
{
    [Required]
    [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
    public string Name { get; set; }

    [Required]
    [StringLength(PhoneNumberMaxLength, MinimumLength = PhoneNumberMinLength)]
    [Display(Name="Phone Number")]
    public string PhoneNumber { get; set; }
}
