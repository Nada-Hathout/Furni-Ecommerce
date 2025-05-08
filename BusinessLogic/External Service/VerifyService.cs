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
    public class VerifyService
    {
        string[] Scopes = { Google.Apis.Gmail.v1.GmailService.Scope.GmailSend };
        string ApplicationName = "Furni Ecommerce Website";

        private string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public void SendEmailConfirmation(string email, string confirmationLink)
        {
            string subject = "Confirm Your Email - Furni Ecommerce";
            string credentialsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "secrets", "credentials.json");

            UserCredential credential;
            using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    ".credentials/furni-gmail-confirm.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            string htmlContent = $@"
<html>
<head>
  <style>
    body {{
      font-family: 'Segoe UI', sans-serif;
      background-color: #f4f4f4;
      padding: 20px;
    }}
    .email-wrapper {{
      max-width: 600px;
      margin: auto;
      background: #ffffff;
      border-radius: 8px;
      box-shadow: 0 2px 6px rgba(0,0,0,0.1);
    }}
    .email-header {{
      background-color: #4CAF50;
      color: white;
      text-align: center;
      padding: 20px;
    }}
    .email-body {{
      padding: 30px;
      color: #333;
      font-size: 16px;
    }}
    .button {{
      display: inline-block;
      background-color: #4CAF50;
      color: white;
      padding: 10px 20px;
      text-decoration: none;
      border-radius: 5px;
    }}
    .footer {{
      font-size: 13px;
      text-align: center;
      color: #999;
      padding: 20px;
      background: #fafafa;
    }}
  </style>
</head>
<body>
  <div class='email-wrapper'>
    <div class='email-header'>
      <h1>Confirm Your Email</h1>
    </div>
    <div class='email-body'>
      <p>Hello,</p>
      <p>Thank you for registering at <strong>Furni Ecommerce</strong>. Please confirm your email address by clicking the button below:</p>
      <p style='text-align: center; margin: 30px 0;'>
        <a class='button' href='{confirmationLink}'>Confirm Email</a>
      </p>
      <p>If you didn't sign up, you can ignore this email.</p>
      <p>Thanks,<br/>Furni Ecommerce Team</p>
    </div>
    <div class='footer'>
      &copy; {DateTime.Now.Year} Furni Ecommerce. All rights reserved.
    </div>
  </div>
</body>
</html>";

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
