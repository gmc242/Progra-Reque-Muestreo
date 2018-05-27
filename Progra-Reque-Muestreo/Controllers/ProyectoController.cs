using Progra_Reque_Muestreo.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    public class ProyectoController : Controller
    {
        // GET: Proyecto
        public ActionResult Index()
        {
            try
            {
                var lista = DatosProyecto.GetProyectosString();
                ViewData["lista_proyectos"] = lista;
                return View("Index");
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Proyecto/Crear
        public ActionResult Crear()
        {
            if (DatosUsuarios.revisarCredenciales("admin"))
            {
                var usuarios = DatosUsuarios.getUsuariosString();
                ViewData["usuarios"] = usuarios;
                return View();
            }
            else
            {
                ViewData["exception"] = new Exception("Solo el usuario administrador puede crear nuevos proyectos.");
                return View("Error");
            }
        }

        // POST: Proyecto/Crear
        [HttpPost]
        public ActionResult Crear(FormCollection collection)
        {
            // Acá llega el collection con la información colectada en el form
            try
            {
                // Parsing de datos de la forma HTML
                String nombre = collection["nombre"];
                int tiempoObs = int.Parse(collection["tiempoObservacion"]);
                var inicio = DateTime.Parse(collection["inicio"]);
                var final = DateTime.Parse(collection["final"]);
                int tamano = int.Parse(collection["tamano"]);
                int tiempoEntreObservaciones = int.Parse(collection["tiempoEntreObservaciones"]);
                String idLider = collection["lider"];
                String descripcion = collection["descripcion"];

                String[] idAsistentesS = null;

                if (collection.AllKeys.Contains("asistentes[]"))
                {
                   idAsistentesS = collection["asistentes[]"].Split(',');
                }

                DatosProyecto.Crear(nombre, tiempoObs, inicio, final, tamano, tiempoEntreObservaciones,
                    idLider, descripcion, idAsistentesS);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["Exception"] = e;
                return View("Error");
            }
        }

        // GET: Proyecto/Modificar/id
        public ActionResult Modificar(int id)
        {
            try
            {
                var dic = DatosProyecto.GetProyecto(id);
                if (dic.ContainsKey("nombre"))
                {
                    if(DatosUsuarios.revisarCredenciales("admin") ||
                        DatosUsuarios.revisarCredenciales(dic["lider_id"]))
                    {
                        ViewData["proyecto"] = dic;
                        var usuarios = DatosUsuarios.getUsuariosString();
                        ViewData["usuarios"] = usuarios;
                        ViewData["id"] = id;
                        return View();
                    }
                    else
                    {
                        throw new Exception("No tiene los privilegios necesarios para ver esta página.");
                    }
                }
                else
                {
                    throw new Exception("No se ha encontrado un proyecto con el id especificado");
                }
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        public ActionResult Info(int id)
        {
            try
            {
                // Se deben revisar credenciales
                var dic = DatosProyecto.GetProyecto(id);
                dic["colaboradores"] = DatosSujeto.GetSujetosDeProyecto(id);
                dic["operaciones"] = DatosActividad.getActividades(id);

                ViewData["proyecto_abierto"] = dic;

                return Index();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult Info(FormCollection collection)
        {
            try
            {
                var idInt = int.Parse(collection["proyecto"]);
                return Info(idInt);
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionColaboradores(int id, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                //int id = int.Parse(idS);

                if (accion.Equals("Agregar"))
                {
                    var nombre = collection["nombreColaborador"];
                    DatosSujeto.Crear(nombre, id);
                    return Info(id);
                }
                else if(accion.Equals("Borrar"))
                {
                    int[] personas = Array.ConvertAll(collection["colaboradores"].Split(','), int.Parse); 
                    foreach(int persona in personas)
                    {
                        DatosSujeto.Eliminar(persona);
                    }
                    return Info(id);
                }
                else
                {
                    throw new Exception("No se ha identificado la acción por ejecutar");
                }
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionOperaciones(int id, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                switch (accion)
                {
                    case "Cargar":
                        {
                            if (collection.AllKeys.Contains("operacion"))
                            {
                                var operacion = int.Parse(collection["operacion"]);
                                var lista = DatosObservacion.GetObservacionesPorActividad(operacion);
                                ViewData["observaciones"] = lista;
                                return Info(id);
                            }
                            else
                            {
                                throw new Exception("Debe escoger una operación para poder cargar la información");
                            }
                        }
                    case "Modificar":
                        {
                            if (collection.AllKeys.Contains("operacion"))
                            {
                                var operacion = int.Parse(collection["operacion"]);
                                return RedirectToAction("Modificar", "Actividad", new { idActividad = operacion });
                            }
                            else
                            {
                                throw new Exception("Debe escoger una operación para poder modificar su información");
                            }
                        }
                    case "Eliminar":
                        {
                            if (collection.AllKeys.Contains("operacion"))
                            {
                                var operacion = int.Parse(collection["operacion"]);
                                DatosActividad.EliminarActividad(operacion);
                                return Info(id);
                            }
                            else
                            {
                                throw new Exception("Debe escoger una operación para poder eliminarla");
                            }
                        }
                    case "Agregar":
                        {
                            return RedirectToAction("Crear", "Actividad", new { id = id });
                        }
                    case "Consolidado":
                        {
                            // Redirige a consolidado
                            return Info(id);
                        }
                    default:
                        {
                            throw new Exception("No se ha podido determinar el tipo de comando por realizar");
                        }
                }
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionObservaciones(int id, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                switch (accion)
                {
                    case "Administrar":
                        {
                            if (collection.AllKeys.Contains("operaciones"))
                            {
                                var lista = Array.ConvertAll(collection["operaciones"].Split(','), int.Parse);
                                if(lista.Length > 1)
                                {
                                    throw new Exception("Solo puede administrar una observación al mismo tiempo");
                                }
                                else
                                {
                                    return RedirectToAction("Detalles", "Observacion", new { id = lista[0] });
                                }
                            }
                            else
                            {
                                throw new Exception("Debe seleccionar alguna observación para administrarla");
                            }
                        }
                    case "Agregar":
                        {
                            return RedirectToAction("Crear", "Observacion", new { idProyecto = id });
                        }
                    case "Eliminar":
                        {
                            if (collection.AllKeys.Contains("operaciones"))
                            {
                                var lista = Array.ConvertAll(collection["operaciones"].Split(','), int.Parse);
                                foreach(int idObs in lista)
                                {
                                    DatosObservacion.Eliminar(idObs);
                                }
                                return Info(id);
                            }
                            else
                            {
                                throw new Exception("Debe seleccionar alguna observación para administrarla");
                            }
                        }
                    default:
                        {
                            throw new Exception("No se ha podido procesar el comando por realizar");
                        }
                }
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionProyecto(int id, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                switch (accion)
                {
                    case "Consolidados y Gráficos":
                        {
                            //RedirectToAction(...) Redirige a consolidados
                            return Info(id);
                        }
                    case "Informe General":
                        {
                            //RedirectToAction(...) Redirige a informe general
                            return Info(id);
                        }
                    case "Informe por Recorridos":
                        {
                            //RedirectToAction(...) Redirige a informe por recorridos
                            return Info(id);
                        }
                    case "Modificar Proyecto":
                        {
                            return Modificar(id);
                        }
                    case "Tipo de muestreo y cálculos":
                        {
                            //RedirectToAction(...) Redirige a página con tipo de muestreo y cálculos
                            return Info(id);
                        }
                    case "Finalizar Proyecto":
                        {
                            //RedirectToAction(...) Hay que modificar base para poder cerrar proyectos
                            return Info(id);
                        }
                    default:
                        {
                            throw new Exception("No se ha podido procesar el comando por ejecutar");
                        }
                }
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Proyecto/Modificar/id
        [HttpPost]
        public ActionResult Modificar(String idProy, FormCollection collection)
        {
            try
            {
                if (DatosUsuarios.revisarCredenciales("admin"))
                {
                    // Parsing de datos de la forma HTML
                    String nombre = collection["nombre"];
                    int tiempoObs = int.Parse(collection["tiempoObservacion"]);
                    var inicio = DateTime.Parse(collection["inicio"]);
                    var final = DateTime.Parse(collection["final"]);
                    int tamano = int.Parse(collection["tamano"]);
                    int tiempoEntreObservaciones = int.Parse(collection["tiempoEntreObservaciones"]);
                    String idLider = collection["lider"];
                    String descripcion = collection["descripcion"];
                    int id = int.Parse(collection["id"]);

                    String[] idAsistentesS = null;

                    if (collection.AllKeys.Contains("asistentes[]"))
                    {
                        idAsistentesS = collection["asistentes[]"].Split(',');
                    }

                    DatosProyecto.Modificar(id, nombre, tiempoObs, inicio, final, tamano, tiempoEntreObservaciones,
                        idLider, descripcion, idAsistentesS);

                    return RedirectToAction("Index");
                }
                else
                {
                    throw new Exception("No tiene los permisos para acceder a esta funcionalidad.");
                }
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET Proyecto/Actividad
        public ActionResult Actividad()
        {
            try
            {
                return View();
            }catch(Exception e)
            {
                return View("Error");
            }
        }

        // GET Proyecto/Actividad/Crear
        public ActionResult CrearActividad()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // POST Proyecto/Actividad/Crear
        [HttpPost]
        public ActionResult CrearActividad(FormCollection form)
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // GET Proyecto/Actividad/Modificar/Id
        public ActionResult ModificarActividad(int id)
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // POST Proyecto/Actividad/Modificar/Id
        [HttpPost]
        public ActionResult ModificarActividad(int id, FormCollection form)
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }

        // GET: Proyecto/Tarea
        public ActionResult Tarea()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return View("Error");
            }
        }


    }
}
