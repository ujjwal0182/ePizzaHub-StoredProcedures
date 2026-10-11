using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.UI.Models.ViewModels
{
    public class AddressViewModel
    {
        [Required]
        public string Street { get; set; } = default!;
        [Required]
        public string Locality { get; set; } = default!;
        [Required]
        [MinLength(6)]
        [MaxLength(6)]
        public string ZipCode { get; set; } = default!;
        [Required]
        public string City { get; set; } = default!;
        [Required]
        public string PhoneNumber { get; set; } = default!;
    }
}
