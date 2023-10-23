using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result GetMail(string email)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"GetEmail '{email}'").AsEnumerable().FirstOrDefault();
                    result.Object = new List<object>();
                    if (query != null) { 
                        ML.Usuario usuario = new ML.Usuario();
                        usuario.Email = query.Email;
                        usuario.Passwords = query.Password;
                        usuario.IdUsuario = query.IdUsuario;
                        usuario.UserName = query.UserName;
                        usuario.Nombre  = query.Nombre;
                        result.Object = usuario;
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct=false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            string messageString = usuario.Password;
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

            //Create the hash value from the array of bytes.
           
            usuario.Passwords = SHA256.HashData(messageBytes);
            try
            {
                using(DL.ImartinezCineContext context = new DL .ImartinezCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}', '{usuario.Email}','{usuario.Nombre}','{usuario.Passwords}'");
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;

        }
        public static ML.Result UpdateContraseña(ML.Usuario usuario)
        {
            ML.Result result= new ML.Result();
            string messageString = usuario.Password;
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageString);

            //Create the hash value from the array of bytes.

            usuario.Passwords = SHA256.HashData(messageBytes);
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdatePassword '{usuario.Email}', '{usuario.Passwords}'");
                    if (query > 0)
                    {
                        result.Correct = true;
                       
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }
            }
            catch(Exception ex) 
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }
    }
}
