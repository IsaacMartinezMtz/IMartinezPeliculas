using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Productos.FromSqlRaw("ProductosGetAll").ToList();
                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Producto producto = new ML.Producto();
                            producto.IdProducto = obj.IdProductos;
                            producto.Nombre = obj.Nombre;
                            producto.Precio = (decimal)obj.Precio;
                            producto.Imagen = obj.Imagen;
                            result.Objects.Add(producto);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetById(int IdProducto)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductosGetById {IdProducto}").AsEnumerable().FirstOrDefault();
                    result.Object = new List<object>();
                    if (query != null)
                    {
                        ML.Producto producto = new ML.Producto();
                        producto.IdProducto = query.IdProductos;
                        producto.Nombre = query.Nombre;
                        producto.Precio = (decimal)query.Precio;
                        producto.Imagen = query.Imagen;

                        result.Object = producto;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct= false;
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
    }
   
}
