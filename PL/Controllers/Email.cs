using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Email : Controller
    {
        public IActionResult VMail()
        {

            return View();
        }
        public IActionResult Form(ML.Email email)
        {
            var send = BL.Email.sendMail(email);
           
            return View();

        }
    }
}
