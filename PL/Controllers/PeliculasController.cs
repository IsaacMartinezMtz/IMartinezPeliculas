using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PL.Models;
using System.Net.Http.Json;

namespace PL.Controllers
{
    public class PeliculasController : Controller
    {
        public IActionResult GetAllPeliPopu(int? page)
        {
         Movie movie = new Movie();
             using (var client  = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                page = page == null ? 1 : page;
                string url = "movie/popular?api_key=f13a3c8901fa3f119654955fdba88ead&language=es-Es&page=" + page;
              
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var resultadoServicio = responseTask.Result;
                if (resultadoServicio.IsSuccessStatusCode)
                {
                    var readTask = resultadoServicio.Content.ReadAsStringAsync();
                    dynamic resultJson = JObject.Parse(readTask.Result);
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    foreach (var item in resultJson.results)
                    {
                        Movie movies = new Movie();
                        movies.IdMovie = item.id;
                        movies.Description = item.overview;
                        movies.Title = item.title;
                        movies.Image = "https://image.tmdb.org/t/p/w600_and_h900_bestv2" + item.poster_path;
                        movies.date = item.release_date;
                        movie.Movies.Add(movies);
                    }
                    //var readTask = resultadoServicio.Content.ReadFromJsonAsync<Models.Result>();
                    //readTask.Wait();

                    //foreach(var item in readTask.Result.Objects) 
                    //{ 
                    //    PL.Models.Peliculas ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<PL.Models.Peliculas>(readTask.Result.Object.ToString());
                    //    root.results.Add(ResultItemList);
                    //}
                }
            }
             ViewBag.page = page;
            return View(movie);
        }
        
        public ActionResult AddPeliculas(int idMovie)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var json = new
                {
                    media_type = "movie",
                    media_id = (int)idMovie,
                    favorite = true,
                };
                var postTask = client.PostAsJsonAsync("account/20522392/favorite?api_key=f13a3c8901fa3f119654955fdba88ead&session_id=29d267ed58400990f5b5503392b7ef45a373f48", json);
                postTask.Wait();

                var result = postTask.Result;
                if(result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Pelicula agregada a tus favoritos";
                }
                else
                {
                    ViewBag.Mensaje = "Error al intentara añadir la pelicula a tus favoritos";
                }
                
            }
            return PartialView("Modal");
        }
        public IActionResult GetAllPeliFav(int? page)
        {
            Movie movie = new Movie();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");

                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                page = page == null ? 1 : page;
                string url = "account/20522392/favorite?api_key=f13a3c8901fa3f119654955fdba88ead&session_id=29d267ed58400990f5b5503392b7ef45a373f48page=" + page;
                //if (page == null)
                //{
                //    url = "movie/popular?api_key=f13a3c8901fa3f119654955fdba88ead&language=es-Es";
                //}
                ////else
                ////{
                ////    url = "movie/popular?api_key=f13a3c8901fa3f119654955fdba88ead&language=es-Es&page";
                ////}
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var resultadoServicio = responseTask.Result;
                if (resultadoServicio.IsSuccessStatusCode)
                {
                    var readTask = resultadoServicio.Content.ReadAsStringAsync();
                    dynamic resultJson = JObject.Parse(readTask.Result);
                    readTask.Wait();
                    movie.Movies = new List<object>();
                    foreach (var item in resultJson.results)
                    {
                        Movie movies = new Movie();
                        movies.IdMovie = item.id;
                        movies.Description = item.overview;
                        movies.Title = item.title;
                        movies.Image = "https://image.tmdb.org/t/p/w600_and_h900_bestv2" + item.poster_path;
                        movies.date = item.release_date;
                        movie.Movies.Add(movies);
                    }
                    //var readTask = resultadoServicio.Content.ReadFromJsonAsync<Models.Result>();
                    //readTask.Wait();

                    //foreach(var item in readTask.Result.Objects) 
                    //{ 
                    //    PL.Models.Peliculas ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<PL.Models.Peliculas>(readTask.Result.Object.ToString());
                    //    root.results.Add(ResultItemList);
                    //}
                }
            }
            ViewBag.page = page;
            return View(movie);
        }
        [HttpPost]
        public ActionResult DeletePeliculas(int idMovie)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
                var json = new
                {
                    media_type = "movie",
                    media_id = (int)idMovie,
                    favorite = false,
                };
                var postTask = client.PutAsJsonAsync($"account/20522392/favorite?api_key=f13a3c8901fa3f119654955fdba88ead?session_id=29d267ed58400990f5b5503392b7ef45a373f48", json);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Mensaje = "Pelicula eliminada de favoritos";
                }
                else
                {
                    ViewBag.Mensaje = "Error al intentara eliminar la pelicula de tus favoritos";
                }

            }
            return PartialView("Modal");
        }
    }
}
