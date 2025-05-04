using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using MimeKit;
using DataAccess.Models;
using System;
namespace Furni_Ecommerce_Website.Controllers
{
    public class ContactController : Controller
    {

        private readonly FurniDbContext _context;

        public ContactController(FurniDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Send(Contact model)
        {
            if (ModelState.IsValid)
            {
                _context.contact.Add(model);
                _context.SaveChanges();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Website Contact Form", "fatma.2002nagy@gmail.com"));
                message.To.Add(new MailboxAddress("Admin", "fatma.2002nagy@gmail.com"));
                message.Subject = "New Contact Form Submission";

                message.Body = new TextPart("plain")
                {
                    Text = $"Name: {model.FName} {model.LName}\nEmail: {model.Email}\nMessage:\n{model.Message}"
                };

                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("fatma.2002nagy@gmail.com", "hvsymvfupszguzfd");
                    client.Send(message);
                    client.Disconnect(true);
                }

                ViewBag.SuccessMessage = "Thanks for your contact with us ";
                ModelState.Clear();  
                return View("Contact");
            }

            return View("Contact", model);
        }

    }

}

