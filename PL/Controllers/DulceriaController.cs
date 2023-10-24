using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using iText.Kernel.Colors;

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

        public ActionResult GenerarPDF()
        {
            ML.Venta venta = new ML.Venta();
            venta.Carrito = new List<object>();
            GetCarrito(venta);

            // Crear un nuevo documento PDF en una ubicación temporal
            string rutaTempPDF = Path.GetTempFileName() + ".pdf";

            using (PdfDocument pdfDocument = new PdfDocument(new PdfWriter(rutaTempPDF)))
            {
                using (Document document = new Document(pdfDocument))
                {
                    document.Add(new Paragraph("Resumen de la Compra"));

                    // Crear la tabla para mostrar la lista de objetos
                    iText.Layout.Element.Table table = new iText.Layout.Element.Table(5); // 5 columnas
                    table.SetWidth(UnitValue.CreatePercentValue(100)); // Ancho de la tabla al 100% del documento

                    // Añadir las celdas de encabezado a la tabla
                    
                    
                    table.AddHeaderCell("ID Producto").SetFontColor(ColorConstants.WHITE);
                    table.AddHeaderCell("Producto").SetBackgroundColor(ColorConstants.BLACK);
                    table.AddHeaderCell("Precio Unitario");
                    table.AddHeaderCell("Cantidad");
                    table.AddHeaderCell("Imagen");
                    

                    


                    foreach (ML.Producto producto in venta.Carrito)
                    {
                        table.AddCell(producto.IdProducto.ToString());
                        table.AddCell(producto.Nombre);
                        table.AddCell(producto.Precio.ToString());
                        table.AddCell(producto.Cantidad.ToString());
                        byte[] imageBytes = Convert.FromBase64String(producto.Imagen);
                        ImageData data = ImageDataFactory.Create(imageBytes);
                        Image image = new Image(data);
                        table.AddCell(image.SetWidth(50).SetHeight(50));

                    }

                    // Añadir la tabla al documento
                    document.Add(table);
                }
            }

            // Leer el archivo PDF como un arreglo de bytes
            byte[] fileBytes = System.IO.File.ReadAllBytes(rutaTempPDF);

            // Eliminar el archivo temporal
            System.IO.File.Delete(rutaTempPDF);
            HttpContext.Session.Remove("Carrito");

            // Descargar el archivo PDF
            return new FileStreamResult(new MemoryStream(fileBytes), "application/pdf")
            {
                FileDownloadName = "ReporteProductos.pdf"
            };
        }
    }
}
