using BusinessLogic.Service;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly IPaymentService _paymentService;
        private readonly ICartItemService _cartItemService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderItemService;

        public CheckoutController(
            IAddressService addressService,
            IPaymentService paymentService,
            ICartItemService cartItemService,
            IOrderService orderService,
            IOrderItemService orderItemService)
        {
            _addressService = addressService;
            _paymentService = paymentService;
            _cartItemService = cartItemService;
            _orderService = orderService;
            _orderItemService = orderItemService;
        }

        // Step 1: show address form
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Step = "Address";
            return View(new CheckoutViewModel());
        }

        // Step 2: validate address and move to payment step
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ValidateAddress(CheckoutViewModel model)
        {
            // remove any validation errors that came from PaymentDetails
            foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PaymentDetails.")).ToList())
                ModelState.Remove(key);

            // now only ShippingAddress.* remains in ModelState
            if (!ModelState.IsValid)
            {
                ViewBag.Step = "Address";
                return View("Index", model);
            }

            TempData["ShippingAddress"] = JsonConvert.SerializeObject(model.ShippingAddress);
            ViewBag.Step = "Payment";
            return View("Index", model);
        }

        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GoBackToAddress(CheckoutViewModel model)
        {
            ModelState.Clear();
            ViewBag.Step = "Address";
            return View("Index", model);
        }

        // to this:
        [HttpGet]
        public IActionResult GoBackToAddress()
        {
            // pull the address back out of TempData
            var addressJson = TempData["ShippingAddress"] as string;
            var vm = new CheckoutViewModel();

            if (!string.IsNullOrEmpty(addressJson))
            {
                vm.ShippingAddress = JsonConvert.DeserializeObject<AddressData>(addressJson)!;
            }

            ViewBag.Step = "Address";
            return View("Index", vm);
        }
        // Step 3: submit order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitOrder(CheckoutViewModel model)
        {
            // Validate the payment details
            if (!ModelState.IsValid)
            {
                ViewBag.Step = "Payment"; // Stay in Payment form if not valid
                return View("Index", model);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            // Retrieve address from TempData
            var addressJson = TempData["ShippingAddress"] as string;
            model.ShippingAddress = JsonConvert.DeserializeObject<AddressData>(addressJson);

            // Save address and payment details
            var address = await _addressService.SaveAddressData(model, userId);
            var payment = await _paymentService.SavePaymentData(model, userId);

            // Get cart items and calculate total
            var cartItems = await _cartItemService.GetAllCartItemsAsc(userId);
            decimal totalAmount = cartItems.Sum(i => i.Quantity * i.Product.Price);

            // Save the order and order items, then clear the cart
            var order =await _orderService.SaveOrderASC(userId, payment.Id, address.Id, totalAmount);
            await _orderItemService.SaveOrderASC(cartItems, order.Id);
            await _cartItemService.RemoveRangeCartItemAsc(cartItems);

            return RedirectToAction("Confirmation");
        }

       
        //// Step 5: confirmation page
        public async Task<IActionResult> Confirmation()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var order = await _orderService.GetOrderByUserId(userId);
                if (order == null) return NotFound("Order not found");

                // Add logging here to verify data at each step
                Console.WriteLine($"Order found: {order.Id}");

                var shippingAddress = await _addressService.GetAddressById(order.AddressId);
                if (shippingAddress == null)
                    return BadRequest("Shipping address not found");

                var payment = await _paymentService.GetPaymentById((int)order.PaymentId);
                if (payment == null)
                    return BadRequest("Payment details not found");

                var cartItems = await _orderItemService.GetOrderItemsByOrderId(order.Id);
                if (cartItems == null || !cartItems.Any())
                    return BadRequest("No items found in order");

                // Get the user ID from the logged-in user
                 userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(); // Ensure the user is authenticated
                }

                // Retrieve the order from the order service (you can replace this with your service call)
                 order = await _orderService.GetOrderByUserId(userId); // Ensure you get the order based on the user
                if (order == null)
                {
                    return NotFound(); // Handle the case where the order does not exist
                }

                // Get the shipping address that was saved during the checkout process
                 shippingAddress = await _addressService.GetAddressById(order.AddressId);

                // Get payment details using the payment service
                 payment = await _paymentService.GetPaymentById((int)order.PaymentId);

                // Get the items in the cart or related to the order
                 cartItems = await _orderItemService.GetOrderItemsByOrderId(order.Id);

                // Calculate the total amount of the order
                decimal totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

                // Prepare the OrderConfirmationViewModel
                var vm = new OrderConfirmationViewModel
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    ShippingAddress = shippingAddress,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.PaymentStatus,
                    TransactionId = payment.TransactionId,
                    Items = cartItems.Select(ci => new OrderItemViewModel
                    {
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.Product.Price
                    }).ToList(),
                    TotalAmount = totalAmount
                };

                // Return the view with the model
                return View("OrderConfirmation", vm);
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, "An error occurred");
            }
        }

    }
}
