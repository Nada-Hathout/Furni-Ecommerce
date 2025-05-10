using BusinessLogic.Service;
using Furni_Ecommerce_Shared.UserViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Security.Claims;

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

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Step = "Address";
        return View(new CheckoutViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ValidateAddress(CheckoutViewModel model)
    {
        foreach (var key in ModelState.Keys.Where(k => k.StartsWith("PaymentDetails.")).ToList())
            ModelState.Remove(key);

        if (!ModelState.IsValid)
        {
            ViewBag.Step = "Address";
            return View("Index", model);
        }

        TempData["ShippingAddress"] = JsonConvert.SerializeObject(model.ShippingAddress);
        ViewBag.Step = "Payment";
        return View("Index", model);
    }

    [HttpGet]
    public IActionResult GoBackToAddress()
    {
        var addressJson = TempData["ShippingAddress"] as string;
        var vm = new CheckoutViewModel();

        if (!string.IsNullOrEmpty(addressJson))
            vm.ShippingAddress = JsonConvert.DeserializeObject<AddressData>(addressJson);

        ViewBag.Step = "Address";
        return View("Index", vm);
    }

    // Stripe session creator
    public async Task<IActionResult> PaymentStripe()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItems = await _cartItemService.GetAllCartItemsAsc(userId);
        var domain = "https://localhost:44332/";

        var options = new SessionCreateOptions
        {
            SuccessUrl = domain + "Checkout/Confirmation?session_id={CHECKOUT_SESSION_ID}",
            CancelUrl = domain + "CartItem/index",
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in cartItems)
        {
            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Product.Price * 100),
                    Currency = "inr",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Name
                    }
                },
                Quantity = item.Quantity
            });
        }

        var service = new SessionService();
        Session session = service.Create(options);
        //TempData["CartData"] = JsonConvert.SerializeObject(cartItems);
        return Redirect(session.Url);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SubmitOrder(CheckoutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Step = "Payment";
            return View("Index", model);
        }

        // Store address in TempData
        TempData["ShippingAddress"] = JsonConvert.SerializeObject(model.ShippingAddress);

        // Redirect to Stripe
        return RedirectToAction("PaymentStripe");
    }

    public async Task<IActionResult> Confirmation(string session_id)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var service = new SessionService();
            var session = service.Get(session_id);

            if (session.PaymentStatus != "paid")
                return RedirectToAction("Index", "CartItem");

            var cartItems = await _cartItemService.GetAllCartItemsAsc(userId);
            var addressJson = TempData["ShippingAddress"] as string;
            var addressData = JsonConvert.DeserializeObject<AddressData>(addressJson);
            var model = new CheckoutViewModel { ShippingAddress = addressData };

            var address = await _addressService.SaveAddressData(model, userId);

            var payment = await _paymentService.SavePaymentData(new CheckoutViewModel
            {
                PaymentDetails = new PaymentData
                {
                    PaymentMethod = "stripe",
                    PaymentStatus = "paid",
                    TransactionId = session.Id
                }
            }, userId);

            decimal totalAmount = cartItems.Sum(i => i.Quantity * i.Product.Price);
            var order = await _orderService.SaveOrderASC(userId, payment.Id, address.Id, totalAmount);
            await _orderItemService.SaveOrderASC(cartItems, order.Id);
            await _cartItemService.RemoveRangeCartItemAsc(cartItems);
            DeleteCartItems();

            return RedirectToAction("OrderConfirmation");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> OrderConfirmation()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var order = await _orderService.GetOrderByUserId(userId);
        var shippingAddress = await _addressService.GetAddressById(order.AddressId);
        var payment = await _paymentService.GetPaymentById((int)order.PaymentId);
        var cartItems = await _orderItemService.GetOrderItemsByOrderId(order.Id);
        var totalAmount = cartItems.Sum(ci => ci.Quantity * ci.Product.Price);
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

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
            Email = userEmail
        };

        _purchaseConfirmationService.PurchaseConfirmationEmail(vm);
        return View("OrderConfirmation", vm);
    }

    public async Task DeleteCartItems()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var cartItems = await _cartItemService.GetAllCartItemsAsc(userId);
        await _cartItemService.RemoveRangeCartItemAsc(cartItems);
    }
}
