using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Observacion/ObservacionTarea")]
    [Route("{action}")]
    public class ObservacionTareaController : Controller
    {
        [Route("Index")]
        // GET: ObservacionTareas
        public ActionResult Index(int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                var id_proyecto = dic_obs["id_proyecto"];
                var sujetosProyecto = DatosSujeto.GetSujetosDeProyecto(id_proyecto);
                var id_actividad = dic_obs["id_actividad"];
                var tareasActividad = DatosTarea.getTareasDeActividad(id_actividad);

                var observacionesTareas = DatosObservacionTarea.GetObservacionTareaPorObservacion(idObservacion);

                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                ViewData["sujetos"] = sujetosProyecto;
                ViewData["tareas"] = tareasActividad;
                ViewData["observacionesTareas"] = observacionesTareas;

                return View();
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }
        
        [Route("Crear")]
        // GET: ObservacionTareas/Crear
        public ActionResult Crear(int idObservacion, int idSujeto, int idTarea)
        {
            try
            {
                DatosObservacionTarea.Crear(idObservacion, idSujeto, idTarea);
                return RedirectToAction("Index", new { idObservacion });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: ObservacionTareas/Delete/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int idObservacionTarea, int idObservacion)
        {
            try
            {
                DatosObservacionTarea.Eliminar(idObservacionTarea);
                return RedirectToAction("Index", new { idObservacion });
            }
            catch(Exception e)
            {
                ViewData["exception"] = new Exception(
                    "No se ha podido eliminar la observación de tarea, revise cualquier tipo de dependencia a esta observacion");
                return View("Error");
            }
        }
    }
}
