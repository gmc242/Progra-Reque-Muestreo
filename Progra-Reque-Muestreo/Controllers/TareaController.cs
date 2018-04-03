using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Actividad/Tarea")]
    [Route("{action}")]
    public class TareaController : Controller
    {
        // GET: Tarea
        [Route("Index")]
        public ActionResult Index(int idActividad)
        {
            try
            {
                var tareas = DatosTarea.getTareasDeActividad(idActividad);
                ViewData["tareas"] = tareas;
                var dicAct = DatosActividad.getActividad(idActividad);
                ViewData["actividad"] = new Tuple<int, String>(dicAct["id_actividad"], dicAct["nombre"]);
                return View();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Tarea/Crear
        [Route("Crear")]
        public ActionResult Crear(int idActividad)
        {
            try
            {
                var dicAct = DatosActividad.getActividad(idActividad);
                ViewData["actividad"] = new Tuple<int, String>(dicAct["id_actividad"], dicAct["nombre"]);
                return View();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Tarea/Crear
        [HttpPost, Route("Crear")]
        public ActionResult Crear(int idActividad, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var descripcion = collection["descripcion"].ToString();
                var idAct = int.Parse(collection["id_actividad"]);
                var categoria = collection["categoria"].ToString();

                DatosTarea.CrearTarea(idAct, nombre, descripcion, categoria);

                return RedirectToAction("Index", new { idActividad });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Tarea/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int idTarea)
        {
            try
            {
                var dic = DatosTarea.getTarea(idTarea);
                ViewData["tarea"] = dic;

                return View();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Tarea/Modificar/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idTarea, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var descripcion = collection["descripcion"].ToString();
                var idAct = int.Parse(collection["id_actividad"]);
                var idTar = int.Parse(collection["id_tarea"]);
                var categoria = collection["categoria"].ToString();

                DatosTarea.ModificarTarea(idTar, idAct, nombre, descripcion, categoria);

                return RedirectToAction("Index", new { idActividad = idAct });
            }
            catch
            {
                return View();
            }
        }

        // GET: Tarea/Eliminar/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int idTarea, int idActividad)
        {
            try
            {
                DatosTarea.EliminarTarea(idTarea);
                return RedirectToAction("Index", new { idActividad });
            }
            catch (Exception e)
            {
                ViewData["exception"] = new Exception("No se ha podido borrar la tarea. Existe una dependencia de esta.\n" +
                    "Se recomienda revisar las observaciones de tareas para borrar toda aquella que está asociada a esta tarea");
                return View("Error");
            }
        }

        
    }
}
