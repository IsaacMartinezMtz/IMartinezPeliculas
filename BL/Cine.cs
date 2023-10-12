using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Cine
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
             using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Cines.FromSqlRaw("CineGetAll").ToList();
                    if (query.Count > 0)
                    {
                        result.Objects = new List<object>();
                        foreach (var obj in query)
                        {
                            ML.Cine cineQ = new ML.Cine();
                            cineQ.IdCine = obj.IdCine;
                            cineQ.Nombre = obj.Nombre;
                            cineQ.Direccion = obj.Direccion;
                            cineQ.Zona = new ML.Zona();
                            cineQ.Zona.IdZona = (int)obj.IdZona;
                            cineQ.Zona.Nombre = obj.NombreZona;
                            cineQ.Ventas = (int)obj.Ventas;

                            result.Objects.Add(cineQ);
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
        public static ML.Result GetById(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Cines.FromSqlRaw($"CineGetById {IdCine}").AsEnumerable().FirstOrDefault();
                    result.Object = new List<object> ();
                    if (query != null)
                    {
                        ML.Cine cine = new ML.Cine();
                        cine.IdCine = query.IdCine;
                        cine.Nombre = query.Nombre;
                        cine.Direccion = query.Direccion;
                        cine.Zona = new ML.Zona();
                        cine.Zona.IdZona = (int)query.IdZona;
                        cine.Zona.Nombre = query.NombreZona;
                        cine.Ventas = (int)query.Ventas;

                        result.Object = cine;
                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct= false;
                    }
                }

            }
            catch(Exception ex)
            {
                result.Correct=false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Add(ML.Cine cine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineAdd '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona},{cine.Ventas}");
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
                result.Correct=false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Update(ML.Cine cine)
        {
            ML.Result result= new ML.Result();
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineUpdate {cine.IdCine}, '{cine.Nombre}', '{cine.Direccion}', {cine.Zona.IdZona}, {cine.Ventas}");
                    if(query > 0)
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
                result.Correct=false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Delete(int IdCine)
        {
            ML.Result result = new ML.Result();
            try
            {
                using(DL.ImartinezCineContext context = new DL.ImartinezCineContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"CineDelete {IdCine}");
                    if(query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct=false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
