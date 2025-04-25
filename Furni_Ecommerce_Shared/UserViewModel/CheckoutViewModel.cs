using System.ComponentModel.DataAnnotations;

namespace Furni_Ecommerce_Shared.UserViewModel
{
    public class CheckoutViewModel
    {
        public AddressData ShippingAddress { get; set; } = new AddressData();
        public PaymentData PaymentDetails { get; set; } = new PaymentData();
        public List<OrderItemViewModel>? OrderItems { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class AddressData
    {
        [Required(ErrorMessage = "Street Address is required.")]
        [MaxLength(200)]
        public string Street { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [MaxLength(100)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required.")]
        [MaxLength(100)]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required.")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Zip code must be 5 digits or in 12345-6789 format.")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [MaxLength(100)]
        public string Country { get; set; }
    }


    public class PaymentData
    {
        [Required(ErrorMessage = "Payment method is required.")]
        [MaxLength(50)]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        [MaxLength(20)]
        public string PaymentStatus { get; set; }

        [MaxLength(100)]
        public string? TransactionId { get; set; }
    }


    public class OrderItemViewModel
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
        public decimal UnitPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }

}