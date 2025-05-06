using Furni_Ecommerce_DashBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BusinessLogic.Service;
using Furni_Ecommerce_Shared.AdminViewModel;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    [Authorize(Roles = "Owner,Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;
        private readonly IProductService _productService;
        private readonly FurniDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IProductService productService,
            FurniDbContext context)
        {
            _logger = logger;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _productService = productService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Total revenue (sum of all order amounts)
            var totalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);

            // Total orders count
            var totalOrders = await _context.Orders.CountAsync();

            // Total registered users
            var totalUsers = await _context.Users.CountAsync();

            // Top 5 selling products
            var topProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .GroupBy(oi => oi.ProductId)
                .Select(g => new ProductSalesData
                {
                    ProductId = g.Key,
                    ProductName = g.First().Product.Name,
                    SalesCount = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .OrderByDescending(p => p.SalesCount)
                .Take(5)
                .ToListAsync();

            // Monthly top products (last 3 months)
            var monthlyTopProducts = new Dictionary<string, List<ProductSalesData>>();
            var months = Enumerable.Range(0, 3).Select(i => DateTime.Now.AddMonths(-i).ToString("MMMM yyyy")).ToList();

            foreach (var month in months)
            {
                var monthNumber = DateTime.ParseExact(month, "MMMM yyyy", null).Month;
                var yearNumber = DateTime.ParseExact(month, "MMMM yyyy", null).Year;

                var products = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Product)
                    .Where(oi => oi.Order.OrderDate.Month == monthNumber && oi.Order.OrderDate.Year == yearNumber)
                    .GroupBy(oi => oi.ProductId)
                    .Select(g => new ProductSalesData
                    {
                        ProductId = g.Key,
                        ProductName = g.First().Product.Name,
                        SalesCount = g.Sum(oi => oi.Quantity),
                        TotalRevenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
                    })
                    .OrderByDescending(p => p.SalesCount)
                    .Take(3)
                    .ToListAsync();

                monthlyTopProducts.Add(month, products);
            }

            var model = new DashboardViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TotalUsers = totalUsers,
                TopProducts = topProducts,
                MonthlyTopProducts = monthlyTopProducts
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}