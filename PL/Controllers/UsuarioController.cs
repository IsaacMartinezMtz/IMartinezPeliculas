using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.Identity.Client;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(ML.Usuario usuario) 
        {
            ML.Result result= BL.Usuario.Add(usuario);
            if (result.Correct)
            {
                ViewBag.Valido = true;
                ViewBag.Mensaje = "Se registro correctamente el usuario";
            }
            else
            {
                ViewBag.Valido = true; 
                ViewBag.Mensaje = "Error al registrar el usuario";
            }
            return PartialView("Modal");
        
        }
        [HttpPost]
        public IActionResult Login(ML.Usuario usuario)
        {
            string email = usuario.UserName;
            string password = usuario.Password;
           
            ML.Result result = BL.Usuario.GetMail(email);

           
            if(result.Correct)
            {
                usuario = (ML.Usuario)result.Object;
                //string pass = usuario.Passwords;
                //byte[] sentHashValue = Convert.FromHexString(pass);
                //string messageString = password;
                //byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);
                //byte[] compareHashValue = SHA256.HashData(messageBytes);
                //bool same = sentHashValue.SequenceEqual(compareHashValue);
                if (password == "12345")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.valido = true;
                    ViewBag.Mensaje = "Error contraseña incorrecta";
                }
            }
            else
            {
                ViewBag.Mensaje = "Error Usuario incorrecto";
                ViewBag.valido = true;
            }
            return PartialView("Modal");
        }
        [HttpGet]
        public IActionResult Recuperar()
        { 
            return View();
        }

            [HttpPost]
        public IActionResult Enviar(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetMail(usuario.Email);
            if (result.Correct)
            {
                
                ML.Email email= new ML.Email();
                email.Subject = "Recuperar Contraseña";
                string path = @"C:\Users\digis\Documents\Isaac Martínez Martínez\IMartinezPeliculas\PL\Views\Usuario\Recuperacion.html";
                using(StreamReader sr = new StreamReader(path))
                {
                    email.Message = sr.ReadToEnd();
                }
                email.Emails = usuario.Email;
                HttpContext.Session.SetString("Email", email.Emails);

                var send = BL.Email.sendMail(email);
                ViewBag.Mensaje = send;
                ViewBag.Valido = true;
                

            }
            else
            {
                ViewBag.Mensaje = "Error Usuario incorrecto";
                ViewBag.valido = true;
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public IActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateC(ML.Usuario usuario)
        {
            usuario.Email= HttpContext.Session.GetString("Email");
            ML.Result result = BL.Usuario.UpdateContraseña(usuario);
            if (result.Correct)
            {
                HttpContext.Session.SetString("Name", null);
                ViewBag.Valido = true;
                ViewBag.Mensaje = "Se Actualizo Correctamente el usuario";

            }
            else
            {
                ViewBag.Valido = false;
                ViewBag.Mensaje = "Error al actalizar el usuario";
            }
            return PartialView("Modal");
        }
    }
}
