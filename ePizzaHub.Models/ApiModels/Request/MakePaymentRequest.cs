using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Models.ApiModels.Request
{
    public class MakePaymentRequest
    {
        [Required] public Guid CartId { get; set; }
        [Required, StringLength(450)] public string RazorpayOrderId { get; set; } = string.Empty;
        [Required, StringLength(450)] public string RazorpayPaymentId { get; set; } = string.Empty;
        [Required, StringLength(500)] public string RazorpaySignature { get; set; } = string.Empty;

        [Required, StringLength(500)] public string Street { get; set; } = string.Empty;
        [Required, StringLength(200)] public string City { get; set; } = string.Empty;
        [Required, StringLength(200)] public string Locality { get; set; } = string.Empty;
        [Required, StringLength(20)] public string ZipCode { get; set; } = string.Empty;
        [Required, Phone, StringLength(20)] public string PhoneNumber { get; set; } = string.Empty;
    }


}
