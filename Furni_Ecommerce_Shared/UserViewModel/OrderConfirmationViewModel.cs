using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.UserViewModel
{
    public class OrderConfirmationViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public AddressData ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string? TransactionId { get; set; }
        public IList<OrderItemViewModel> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public string Email {  get; set; }
    }
    public class OrderItemsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
