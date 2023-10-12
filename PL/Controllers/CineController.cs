using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PL.Models;
using System.Drawing.Drawing2D;
using System.Security.Policy;

namespace PL.Controllers
{
    public class CineController : Controller
    {
        public IActionResult GetAll()
        {
            ML.Cine cine = new ML.Cine();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5087/api/Cine/");
                var responseTask = client.GetAsync("GetAll/");
                responseTask.Wait();
                var resultadoServicio = responseTask.Result;
                if (resultadoServicio.IsSuccessStatusCode)
                {
                    var readTask = resultadoServicio.Content.ReadAsStringAsync();
                    dynamic resultJ = JObject.Parse(readTask.Result);
                    readTask.Wait();
                    cine.Cines = new List<object>();

                    foreach (var item in resultJ.objects)
                    {
                        ML.Cine cines = new ML.Cine();
                        cines.IdCine = item.idCine;
                        cines.Nombre = item.nombre;
                        cines.Direccion = item.direccion;
                        cines.Zona = new ML.Zona();
                        cines.Zona.IdZona = item.zona.idZona;
                        cines.Zona.Nombre = item.zona.nombre;
                        cines.Ventas = item.ventas;
                        cine.Cines.Add(cines);
                    }
                }
            }
          return View(cine);
        }
        [HttpGet]
        public IActionResult Form(int? IdCine)
        {

            ML.Cine cine = new ML.Cine();
           
            
            if (IdCine != null)
            {
                cine.IdCine = (int)IdCine;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:5087/api/Cine/");
                    var responseTask = client.GetAsync($"GetById/{IdCine}");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadFromJsonAsync<ML.Result>();
                        readTask.Wait();

                        ML.Cine resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Cine>(readTask.Result.Object.ToString());
                        cine = resultItemList;
                    }
                    return View(cine);
                }
            }
            else
            {

            }
            return View(cine);
        }
        [HttpPost]
        public IActionResult Form(ML.Cine cine)
        {
            if (cine.IdCine == 0)
            {
                ML.Result result = new ML.Result();
                using (var client = new HttpClient())
                {
                    //client.BaseAddress = new Uri("http://localhost:5087/api/Cine/");
                    var postTask = client.PostAsJsonAsync("http://localhost:5087/api/Cine/Add", cine);
                    postTask.Wait();

                    var resultServicio = postTask.Result;
                    if (resultServicio.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se ha registrado correctamente El cine";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Error al registrar el cine";
                    }

                }
            }
            else
            {

                ML.Result result = new ML.Result();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:8115/api/");
                    var postTask = client.PutAsJsonAsync<ML.Cine>("Update", cine);
                    postTask.Wait();

                    var resultServicio = postTask.Result;
                    if (resultServicio.IsSuccessStatusCode)
                    {
                        ViewBag.Mensaje = "Se ha registrado correctamente la materia";
                    }
                    else
                    {
                        ViewBag.Mensaje = "Error al registrar la materia";
                    }
                }
            }
            return PartialView("Modal");

        }
        [HttpGet]
        public IActionResult Delete(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
             int id = cine.IdCine;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5087/api/Cine/");
                var postTask = client.DeleteAsync("Delete/" + id);
                postTask.Wait();

                var resultServicio = postTask.Result;
                if (resultServicio.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Se ha eliminado correctamente el cine";
                }
                else
                {
                    ViewBag.Mensaje = "Error al Eliminar el cine";
                }
            }
            return PartialView("Modal");
        }
    }
}
