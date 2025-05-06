using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class ProductSalesData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
