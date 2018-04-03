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
                return View();
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
