using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic.External_Service
{
    public class GmailService
    {
        string[] Scopes = { Google.Apis.Gmail.v1.GmailService.Scope.GmailSend };
        string ApplicationName = "Furni Ecommerce Website";

        // Method to encode the email content to Base64URL
        private string Base64UrlEncode(string input)
        {
            var data = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(data)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        // Method to send notification emails with updated content related to a purchase
        public void SendPurchaseConfirmation(string email, string orderDetails)
        {
            string subject = "Thank You for Your Purchase at Furni Ecommerce";
            UserCredential credential;
            var credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "secrets", "credentials.json");

            // Load credentials from the credentials.json file
            using (FileStream stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Define the HTML content of the email
            string htmlContent = $@"
<html>
<head>
  <style>
    body {{
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
      background-color: #f4f4f4;
      padding: 20px;
      margin: 0;
    }}
    .email-wrapper {{
      max-width: 600px;
      margin: auto;
      background: #ffffff;
      border-radius: 8px;
      box-shadow: 0 2px 6px rgba(0,0,0,0.1);
      overflow: hidden;
    }}
    .email-header {{
      background-color: #2c3e50;
      color: white;
      text-align: center;
      padding: 20px;
    }}
    .email-header h1 {{
      margin: 0;
      font-size: 24px;
    }}
    .email-body {{
      padding: 30px;
      color: #333;
      font-size: 16px;
    }}
    .email-body h2 {{
      color: #2c3e50;
    }}
    .order-summary {{
      background-color: #f9f9f9;
      padding: 15px;
      border: 1px solid #e0e0e0;
      border-radius: 6px;
      margin: 20px 0;
    }}
    .order-summary p {{
      margin: 6px 0;
    }}
    .footer {{
      font-size: 13px;
      text-align: center;
      color: #999;
      padding: 20px;
      background: #fafafa;
    }}
    ul {{
      padding-left: 20px;
    }}
    li {{
      margin-bottom: 8px;
    }}
  </style>
</head>
<body>
  <div class='email-wrapper'>
    <div class='email-header'>
      <h1>Thank You for Shopping at Furni Ecommerce</h1>
    </div>
    <div class='email-body'>
      <p>Dear Customer,</p>
      <p>Thank you for your recent purchase! Your order has been received and is now being processed. Below are the details:</p>

      <div class='order-summary'>
        {orderDetails}
      </div>

      <p>If you have any questions, feel free to reach out to our support team. We’re here to help!</p>
      <p>We’ll notify you as soon as your order ships.</p>
      <p>Best regards,</p>
      <p><strong>Furni Ecommerce Team</strong></p>
    </div>
    <div class='footer'>
      &copy; {DateTime.Now.Year} Furni Ecommerce. All rights reserved.
    </div>
  </div>
</body>
</html>";


            // Construct the email message
            string message = $"To: {email}\r\n" +
                             $"Subject: {subject}\r\n" +
                             "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                             htmlContent;

            // Initialize the Gmail API service
            var service = new Google.Apis.Gmail.v1.GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Encode the message and send it
            var msg = new Google.Apis.Gmail.v1.Data.Message
            {
                Raw = Base64UrlEncode(message)
            };
            service.Users.Messages.Send(msg, "me").Execute();

            Console.WriteLine("Your email has been successfully sent!");
        }
    }
}
