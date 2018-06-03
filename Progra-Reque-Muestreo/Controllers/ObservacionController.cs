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
            var lista = DatosActividad.getActividades(idProyecto);
            ViewData["operaciones"] = lista;
            var dic = DatosProyecto.GetProyecto(idProyecto);
            ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
            return View("Index");
        }

        [HttpPost]
        public ActionResult AccionOperacion(int idProy, FormCollection collection)
        {
            try
            {
                var operacion = int.Parse(collection["operacion"]);
                var obs = DatosObservacion.GetObservacionesPorActividad(operacion);
                ViewData["operacion_sel"] = operacion;
                ViewData["observaciones"] = obs;
                return Index(idProy);
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionObservaciones(int idProy, int idOp, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                switch (accion)
                {
                    case "Cargar":
                        {
                            var idObs = int.Parse(collection["observacion"]);
                            return Detalles(idProy, idOp, idObs);
                        }
                    case "Agregar":
                        {
                            return Crear(idProy, idOp);
                        }
                    case "Eliminar":
                        {
                            var idObs = int.Parse(collection["observacion"]);
                            DatosObservacion.Eliminar(idObs);
                            ViewData["operacion_sel"] = idOp;
                            var observaciones = DatosObservacion.GetObservacionesPorActividad(idOp);
                            ViewData["observaciones"] = observaciones;
                            return Index(idProy);
                        }
                    case "Modificar":
                        {
                            var idObs = int.Parse(collection["observacion"]);
                            return Modificar(idObs, idOp, idProy);
                        }
                    default:
                        {
                            return Index(idProy);
                        }
                    
                }
                
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        public ActionResult Detalles(int idProy, int idOp, int idObs)
        {
            try
            {
                // Primeros datos
                var obsLista = DatosObservacion.GetObservacionesPorActividad(idOp);
                ViewData["operacion_sel"] = idOp;
                ViewData["observaciones"] = obsLista;

                //Parsea observacion seleccionada
                var obs = DatosObservacion.GetObservacion(idObs);
                ViewData["observacion_sel"] = idObs;
                ViewData["observacion"] = obs;

                var obsTarea = DatosObservacionTarea.GetObservacionTareaPorObservacion(idObs);
                ViewData["obs_tarea"] = obsTarea;

                ViewData["status"] = ControladorGlobal.ObtenerStatusCantidadString(idOp, idProy);
                ViewData["colaboradores"] = DatosSujeto.GetSujetosDeProyecto(idProy);
                ViewData["tareas"] = DatosTarea.getTareasDeActividad(idOp);
                ViewData["recorridos"] = DatosRonda.GetRondasDeObservacion(idObs);

                return Index(idProy);
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AccionRecorrido(int idProy, int idOp, int idObs, FormCollection collection)
        {
            try
            {
                var recorrido = int.Parse(collection["recorrido"]);
                return RedirectToAction("Detalles", "Ronda",
                    new { idProy = idProy, idOp = idOp, idObs = idObs, idRonda = recorrido });
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Observación/Crear
        [Route("Crear")]
        public ActionResult Crear(int idProyecto, int idOp)
        {
            var listaActividades = DatosActividad.getActividades(idProyecto);
            ViewData["actividades"] = listaActividades;
            var dic = DatosProyecto.GetProyecto(idProyecto);
            ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
            ViewData["id_op"] = idOp;
            return View("Crear");
        }

        // POST: Observación/Create
        [HttpPost] [Route("Crear")]
        public ActionResult Crear(int idProyecto, int idOp, FormCollection collection)
        {
            try
            {
                var descripcion = collection["descripcion"];
                var idActividad = int.Parse(collection["actividad"]);
                var dia = DateTime.Parse(collection["dia"]);

                var res = DatosObservacion.Crear(idActividad, descripcion, dia);

                return Detalles(idProyecto, idOp, res);
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Observación/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int idObservacion, int idOp, int idProyecto)
        {
            try
            {
                var listaActividades = DatosActividad.getActividades(idProyecto);

                ViewData["actividades"] = listaActividades;
                var dic = DatosProyecto.GetProyecto(idProyecto);

                ViewData["proyecto"] = new Tuple<int, String>(idProyecto, dic["nombre"]);
                ViewData["observacion"] = DatosObservacion.GetObservacion(idObservacion);

                ViewData["id_op"] = idOp;

                return View("Modificar");
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Observación/Edit/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idObservacion, int idOp, int idProyecto, FormCollection collection)
        {
            try
            {
                var idActividad = int.Parse(collection["actividad"]);
                var descripcion = collection["descripcion"];
                var dia = DateTime.Parse(collection["dia"]);
                var idObs = int.Parse(collection["id_observacion"]);

                DatosObservacion.Modificar(idObs, idActividad, descripcion, dia);

                return Detalles(idProyecto, idOp, idProyecto);
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
