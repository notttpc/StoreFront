using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using StoreFront.DATA.EF.Models;
using StoreFront.UI.MVC.Models;
using System.Diagnostics;
using static Org.BouncyCastle.Math.EC.ECCurve;


namespace StoreFront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly AnimeShopContext _context;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, AnimeShopContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }


        public IActionResult Index()
        {
            var animeShopContext = _context.Products.Where(p => p.IsFeatured)
                .Include(p => p.Category).Include(p => p.Company).Include(p => p.Genre).Include(p => p.ProductStatus).Include(p => p.Sword);
            return View(animeShopContext.ToList());
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string message = $"You have received a new email from your store contact form!<br />" +
                $"Sender: {cvm.Name}<br />Email: {cvm.Email}<br />Subject: {cvm.Subject}<br />" +
                $"Message: {cvm.Message}";
            var mm = new MimeMessage();

            mm.From.Add(new MailboxAddress("Sender", _config.GetValue<string>("Credentials:Email:User")));

            mm.To.Add(new MailboxAddress("Personal", _config.GetValue<string>("Credentials:Email:Recipient")));

            mm.Subject = cvm.Subject;

            mm.Body = new TextPart("HTML") { Text = message };

            mm.Priority = MessagePriority.Urgent;

            mm.ReplyTo.Add(new MailboxAddress("User", cvm.Email));
            using (var client = new SmtpClient())
            {

                client.Connect(_config.GetValue<string>("Credentials:Email:Client"), 8889);

                client.Authenticate
                (

                //username
                    _config.GetValue<string>("Credentials:Email:User"),

                //password
                    _config.GetValue<string>("Credentials:Email:Password")

                     );


                try
                {
                    //try to send the email
                    client.Send(mm);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"There was an error processing your request. Please try again later." +
                        $"<br />Error Message: {ex.StackTrace}";

                    return View(cvm);

                    //throw;
                }

            }//client is disposed


            return View("EmailConfirmation", cvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}