using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DulceriaController : Controller
    {
        public IActionResult GetAll()
        {

            return View();
        }
        [HttpGet]
        public IActionResult ProductosGetAll()
        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = BL.Producto.GetAll();

            if (result.Correct)
            {
                producto.Productos = result.Objects;
            }
            else
            {
                return View();
            }
            return View(producto);
        }
        public IActionResult AddCarrito(int idProducto)
        {
            bool existe = false;
            ML.Venta carrito= new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Producto.GetById(idProducto);

            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Producto producto = (ML.Producto)result.Object;
                    producto.Cantidad = 1;
                    carrito.Carrito.Add(producto);
                    //serializar correcto
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));

                }
            }
            else
            {
                ML.Producto producto=(ML.Producto)result.Object;
                GetCarrito(carrito); //se recupera el carrito
                foreach (ML.Producto productoM in carrito.Carrito)
                {
                    if (producto.IdProducto == productoM.IdProducto)
                    {
                        productoM.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                    if (existe == true)
                    {
                        HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                    }
                    else
                    {
                        producto.Cantidad = 1;
                        carrito.Carrito.Add(producto);
                        HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                    }
                }

            return RedirectToAction("ProductosGetAll");
        }
        public ML.Venta GetCarrito(ML.Venta carrito)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Carrito"));
            foreach(var obj in ventaSession)
            {
                ML.Producto objMateria = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                carrito.Carrito.Add(objMateria);
            }
            return carrito;
        }
        public ActionResult carrito()
        {
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            if(HttpContext.Session.GetString("Carrito") == null)
            {
                return View(carrito);
            }
            else
            {
                GetCarrito(carrito);
                return View(carrito);
            }
        }
        public ActionResult DeleteCarro()
        {
            if (HttpContext.Session.GetString("Carrito") != null)
            {
                HttpContext.Session.Remove("Carrito");
            }
            return RedirectToAction("ProductosGetAll");
        }
        public ActionResult AgregarElemento(int idProducto)
        {
            bool existe = false;
            ML.Venta carrito = new ML.Venta();
            carrito.Carrito = new List<object>();
            ML.Result result = BL.Producto.GetById(idProducto);

            if (HttpContext.Session.GetString("Carrito") == null)
            {
                if (result.Correct)
                {
                    ML.Producto producto = (ML.Producto)result.Object;
                    producto.Cantidad = 1;
                    carrito.Carrito.Add(producto);
                    //serializar correcto
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));

                }
            }
            else
            {
                ML.Producto producto = (ML.Producto)result.Object;
                GetCarrito(carrito); //se recupera el carrito
                foreach (ML.Producto productoM in carrito.Carrito)
                {
                    if (producto.IdProducto == productoM.IdProducto)
                    {
                        productoM.Cantidad += 1;
                        existe = true;
                        break;
                    }
                    else
                    {
                        existe = false;
                    }
                }
                if (existe == true)
                {
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
                else
                {
                    producto.Cantidad = 1;
                    carrito.Carrito.Add(producto);
                    HttpContext.Session.SetString("Carrito", Newtonsoft.Json.JsonConvert.SerializeObject(carrito.Carrito));
                }
            }

            return RedirectToAction("carrito");
        }
        
        public ActionResult EliminarProductos(int idProducto)
        {
            return View();
        }
    }
}
