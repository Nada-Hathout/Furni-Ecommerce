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

        body.Append(@"
    <div style='font-family:Segoe UI,Roboto,sans-serif; max-width:600px; margin:auto; padding:20px; background-color:#f7f7f7; border:1px solid #ddd; border-radius:10px;'>
        <div style='background-color:#4CAF50; padding:15px 20px; color:white; border-radius:10px 10px 0 0;'>
            <h2 style='margin:0;'>Furni Ecommerce - Order Confirmation</h2>
        </div>

        <div style='padding:20px; background-color:white; border-radius:0 0 10px 10px;'>");

        // Order Info
        body.Append($@"
            <p style='font-size:16px;'>Hi,</p>
            <p>Thank you for your purchase! Below are the details of your order.</p>

            <p><strong>Order Number:</strong> #{order.OrderId}</p>
            <p><strong>Order Date:</strong> {order.OrderDate:MMMM dd, yyyy}</p>");

        // Shipping Address
        body.Append(@"
            <h3 style='border-bottom:1px solid #eee; padding-bottom:5px;'>Shipping Address</h3>");
        body.Append($@"
            <p>
                {order.ShippingAddress.Street}<br>
                {order.ShippingAddress.City}, {order.ShippingAddress.State} {order.ShippingAddress.ZipCode}<br>
                {order.ShippingAddress.Country}
            </p>");

        // Payment Info
        body.Append(@"
            <h3 style='border-bottom:1px solid #eee; padding-bottom:5px;'>Payment Details</h3>");
        body.Append($@"
            <p>
                <strong>Method:</strong> {order.PaymentMethod}<br>
                <strong>Status:</strong> {order.PaymentStatus}<br>
                <strong>Transaction ID:</strong> {order.TransactionId}
            </p>");

        // Items
        body.Append(@"
            <h3 style='border-bottom:1px solid #eee; padding-bottom:5px;'>Items Purchased</h3>
            <table style='width:100%; border-collapse:collapse;'>
                <thead>
                    <tr>
                        <th align='left' style='border-bottom:1px solid #ccc; padding:8px;'>Product</th>
                        <th align='center' style='border-bottom:1px solid #ccc; padding:8px;'>Qty</th>
                        <th align='right' style='border-bottom:1px solid #ccc; padding:8px;'>Price</th>
                    </tr>
                </thead>
                <tbody>");

        foreach (var item in order.Items)
        {
            body.Append($@"
                    <tr>
                        <td style='padding:8px; border-bottom:1px solid #eee;'>{item.ProductName}</td>
                        <td align='center' style='padding:8px; border-bottom:1px solid #eee;'>{item.Quantity}</td>
                        <td align='right' style='padding:8px; border-bottom:1px solid #eee;'>${item.UnitPrice:F2}</td>
                    </tr>");
        }

        body.Append(@"
                </tbody>
            </table>");

        // Total
        body.Append($@"
            <p style='margin-top:20px; font-size:16px;'><strong>Total Amount:</strong> ${order.TotalAmount:F2}</p>

            <p style='margin-top:30px;'>We’ll notify you as soon as your items are shipped. Thank you for choosing <strong>Furni Ecommerce</strong>.</p>

            <p style='color:#555; font-size:14px;'>— The Furni Ecommerce Team</p>
        </div>
    </div>");

        // Send it using GmailService
        gmailService.SendPurchaseConfirmation(order.Email, body.ToString());
    }

}
