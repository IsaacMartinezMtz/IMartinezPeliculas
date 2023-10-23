using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace BL
{
    public class Email
    {
        public Task SendEmailAsync(ML.Email email)
        {
            try
            {
                email.Subject = "Recuperacion de Contraseña";
               

                var credentials = new NetworkCredential("isaamarti89@gmail.com", "V1LvtFzYOSwx4Bqg");
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("isaamarti89@gmail.com", "Isaac"),
                    Subject = email.Subject,
                    Body = email.Message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email.Emails));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp-relay.brevo.com",
                    EnableSsl = false,
                    Credentials = credentials
                };

                // Send it...         
                client.Send(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

            return Task.CompletedTask;
        }
        //    public void EnviarCorreo(string destinatario, string asunto, string mensaje)
        //    {
        //        var correo = new MailMessage();
        //        correo.From = new mailaddress("tu_correo@gmail.com");

        //        correo.To.Add(destinatario);
        //        correo.Subject = asunto;
        //        correo.Body = mensaje;
        //        correo.IsBodyHtml = true;
        //        correo.Priority = MailPriority.Normal;

        //        var smtp = new SmtpClient();
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new networkcredential("tu_correo@gmail.com", "tu_contraseña");

        //try
        //        {
        //            smtp.Send(correo);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }

        
        public static string sendMail(ML.Email email)
        {
            string msge = "Error al enviar este correo. Por favor verifique los datos o intente más tarde.";
            string from = "isaamarti89@gmail.com";
            string displayName = "Isaac";
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(from, displayName);
                mail.To.Add(email.Emails);

                mail.Subject = email.Subject;
                mail.Body = email.Message;
                mail.IsBodyHtml = true;


                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Aquí debes sustituir tu servidor SMTP y el puerto
                
                client.EnableSsl = true;//En caso de que tu servidor de correo no utilice cifrado SSL,poner en false
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(from, "idlamsreoxnotops");
                
               


                client.Send(mail);
                msge = "¡Correo enviado exitosamente! Pronto te contactaremos.";

            }
            catch (Exception ex)
            {
                msge = ex.Message + ". Por favor verifica tu conexión a internet y que tus datos sean correctos e intenta nuevamente.";
            }

            return msge;
        }


    }
}
