using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class DashboardViewModel
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUsers { get; set; }
        public List<ProductSalesData> TopProducts { get; set; }
        public Dictionary<string, List<ProductSalesData>> MonthlyTopProducts { get; set; }
    }
}
