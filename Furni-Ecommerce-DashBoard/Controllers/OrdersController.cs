using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Furni_Ecommerce_DashBoard.Controllers
{
    [Authorize(Roles = "Owner,Admin")]
    public class OrdersController : Controller
    {
        private readonly FurniDbContext _context;
        private const int PageSize = 10;

        public OrdersController(FurniDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? pageNumber, string searchString, DateTime? fromDate, DateTime? toDate)
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o =>
                    o.User.UserName.Contains(searchString) ||
                    o.User.FirstName.Contains(searchString) ||
                    o.User.LastName.Contains(searchString));
            }

            if (fromDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= toDate.Value);
            }

            // Order by date descending
            orders = orders.OrderByDescending(o => o.OrderDate);

            var paginatedOrders = await PaginatedList<Order>.CreateAsync(orders.AsNoTracking(), pageNumber ?? 1, PageSize);

            var orderViewModels = paginatedOrders.Select(o => new OrderViewModel
            {
                Id = o.Id,
                CustomerName = $"{o.User.FirstName} {o.User.LastName}",
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status
            }).ToList();

            var model = new OrderListViewModel
            {
                Orders = new PaginatedList<OrderViewModel>(orderViewModels, paginatedOrders.TotalCount, paginatedOrders.PageIndex, PageSize),
                SearchString = searchString,
                FromDate = fromDate,
                ToDate = toDate
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var items = order.OrderItems.Select(oi => new OrderItemViewModel
            {
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                Price = oi.UnitPrice
            }).ToList();

            var model = new OrderDetailViewModel
            {
                OrderId = order.Id,
                CustomerName = $"{order.User.FirstName} {order.User.LastName}",
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Items = items
            };

            return View(model);
        }
    }
}