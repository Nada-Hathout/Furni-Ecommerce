using System;
using System.Text;
using BusinessLogic.External_Service;
using System.Linq;
using BusinessLogic.Service;
using Furni_Ecommerce_Shared.UserViewModel;

public class PurchaseConfirmationService : IPurchaseConfirmationService
{
    private readonly GmailService gmailService;

    // Constructor to inject the Gmail service
    public PurchaseConfirmationService(GmailService gmailService)
    {
        this.gmailService = gmailService;
    }

    // Method to send the purchase confirmation email
    public void PurchaseConfirmationEmail(OrderConfirmationViewModel order)
    {
        var body = new StringBuilder();

        body.Append($"<p><strong>Order Number:</strong> #{order.OrderId}</p>");
        body.Append($"<p><strong>Order Date:</strong> {order.OrderDate:MMMM dd, yyyy}</p>");

        // Shipping Address
        body.Append("<h4>Shipping Address</h4>");
        body.Append($"<p>{order.ShippingAddress.Street}<br>");
        body.Append($"{order.ShippingAddress.Street}, {order.ShippingAddress.City}<br>");
        body.Append($"{order.ShippingAddress.State}, {order.ShippingAddress.ZipCode}<br>");
        body.Append($"{order.ShippingAddress.Country}</p>");

        // Payment Information
        body.Append("<h4>Payment Details</h4>");
        body.Append($"<p><strong>Method:</strong> {order.PaymentMethod}<br>");
        body.Append($"<strong>Status:</strong> {order.PaymentStatus}<br>");
        body.Append($"<strong>Transaction ID:</strong> {order.TransactionId}</p>");

        // Items
        body.Append("<h4>Items Purchased</h4><ul>");
        foreach (var item in order.Items)
        {
            body.Append($"<li>{item.ProductName} (x{item.Quantity}) - ${item.UnitPrice:F2}</li>");
        }
        body.Append("</ul>");

        // Total
        body.Append($"<p><strong>Total Amount:</strong> ${order.TotalAmount:F2}</p>");

        // Send it using GmailService
        gmailService.SendPurchaseConfirmation(order.Email, body.ToString());
    }
}
