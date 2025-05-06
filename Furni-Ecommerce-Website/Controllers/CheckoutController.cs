using BusinessLogic.Service;
using DataAccess.Models;
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
        private readonly IPurchaseConfirmationService _purchaseConfirmationService;

        public CheckoutController(
            IAddressService addressService,
            IPaymentService paymentService,
            ICartItemService cartItemService,
            IOrderService orderService,
            IOrderItemService orderItemService,
            IPurchaseConfirmationService purchaseConfirmationService)
        {
            _addressService = addressService;
            _paymentService = paymentService;
            _cartItemService = cartItemService;
            _orderService = orderService;
            _orderItemService = orderItemService;
            _purchaseConfirmationService = purchaseConfirmationService;
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

            DeleteCartItems();
            return RedirectToAction("Confirmation");
        }

        public async void DeleteCartItems()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var cartItems = await _cartItemService.GetAllCartItemsAsc(userId);



        }

        //// Step 5: confirmation page
        public async Task<IActionResult> Confirmation()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                // Get the order based on the userId
                var order = await _orderService.GetOrderByUserId(userId);
                if (order == null) return NotFound("Order not found");

                // Log order details for debugging purposes

                // Retrieve shipping address
                var shippingAddress = await _addressService.GetAddressById(order.AddressId);
                if (shippingAddress == null)
                    return BadRequest("Shipping address not found");

                // Retrieve payment details
                var payment = await _paymentService.GetPaymentById((int)order.PaymentId);
                if (payment == null)
                    return BadRequest("Payment details not found");

                // Retrieve the items associated with the order
                var cartItems = await _orderItemService.GetOrderItemsByOrderId(order.Id);
                if (cartItems == null || !cartItems.Any())
                    return BadRequest("No items found in order");

                // Calculate total amount
                decimal totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);

                // Get the user's email address
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized(); // Ensure the user has an email
                }

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
                    TotalAmount = totalAmount,
                    Email = userEmail // Assign the user's email
                };

                // Send confirmation email (if required)
                _purchaseConfirmationService.PurchaseConfirmationEmail(vm);
                // Return the view with the model
                return View("OrderConfirmation", vm);
            }
            catch (Exception ex)
            {
                // Log the error details for better debugging
                return StatusCode(500,
       $"An error occurred while processing your request. Error: {ex.Message} {(ex.InnerException != null ? "Inner: " + ex.InnerException.Message : "")}");

            }
        }


    }
}
