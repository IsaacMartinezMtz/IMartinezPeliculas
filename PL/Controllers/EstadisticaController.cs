using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace PL.Controllers
{
    public class EstadisticaController : Controller
    {
        public IActionResult Estadistica()
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
    }
}
