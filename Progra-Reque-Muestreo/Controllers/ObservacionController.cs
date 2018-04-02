using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Observacion")]
    [Route("{action}")]
    public class ObservacionController : Controller
    {
        [Route("Index")]
        // GET: Observación
        public ActionResult Index(int idProyecto)
        {
            var lista = DatosObservacion.GetObservacionesPorProyecto(idProyecto);
            ViewData["observaciones"] = lista;
            var dic = DatosProyecto.GetProyecto(idProyecto);
            ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
            return View();
        }

        // GET: Observación/Crear
        [Route("Crear")]
        public ActionResult Crear(int idProyecto)
        {
            var listaActividades = DatosActividad.getActividades(idProyecto);
            ViewData["actividades"] = listaActividades;
            var dic = DatosProyecto.GetProyecto(idProyecto);
            ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
            return View();
        }

        // POST: Observación/Create
        [HttpPost] [Route("Crear")]
        public ActionResult Crear(int idProyecto, FormCollection collection)
        {
            try
            {
                var descripcion = collection["descripcion"];
                var idActividad = int.Parse(collection["actividad"]);
                var dia = DateTime.Parse(collection["dia"]);

                DatosObservacion.Crear(idActividad, descripcion, dia);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Observación/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int idObservacion, int idProyecto)
        {
            try
            {
                var listaActividades = DatosActividad.getActividades(idProyecto);

                ViewData["actividades"] = listaActividades;
                var dic = DatosProyecto.GetProyecto(idProyecto);

                ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
                ViewData["observacion"] = DatosObservacion.GetObservacion(idObservacion);

                return View();
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Observación/Edit/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idObservacion, int idProyecto, FormCollection collection)
        {
            try
            {
                var idActividad = int.Parse(collection["actividad"]);
                var descripcion = collection["descripcion"];
                var dia = DateTime.Parse(collection["dia"]);
                var idObs = int.Parse(collection["id_observacion"]);

                DatosObservacion.Modificar(idObs, idActividad, descripcion, dia);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Observación/Delete/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int idObservacion, int idProyecto)
        {
            try
            {
                DatosObservacion.Eliminar(idObservacion);
                return RedirectToAction("Index", new { idProyecto });
            }
            catch
            {
                ViewData["exception"] = new Exception("No se ha podido eliminar la observacion, " +
                    "revise que no haya dependencias en las rondas y observaciones de tareas.");
                return View("Error");
            }
        }

        
    }
}
