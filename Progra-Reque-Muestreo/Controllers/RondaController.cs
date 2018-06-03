using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Observacion/Ronda")]
    [Route("{action}")]
    public class RondaController : Controller
    {
        // GET: Ronda
        [Route("Index")]
        public ActionResult Index(int idProy)
        {
            try
            {
                var ops = DatosActividad.getActividades(idProy);
                var proyecto = DatosProyecto.GetProyecto(idProy);
                ViewData["operaciones"] = ops;
                var dic = DatosProyecto.GetProyecto(idProy);
                ViewData["proyecto"] = new Tuple<int, String>(idProy, dic["nombre"]);
                return View("Index");
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost, Route("AccionOperaciones")]
        public ActionResult AccionOperaciones(int idProy, FormCollection collection)
        {
            try
            {
                var op = int.Parse(collection["operacion"]);
                ViewData["operacion_sel"] = op;
                var obs = DatosObservacion.GetObservacionesPorActividad(op); 
                ViewData["observaciones"] = obs;
                return Index(idProy);
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost, Route("AccionObservacion")]
        public ActionResult AccionObservacion(int idProy, int idOp, FormCollection collection)
        {
            try
            {
                var obs = int.Parse(collection["observacion"]);
                ViewData["observacion_sel"] = obs;
                ViewData["recorridos"] = DatosRonda.GetRondasDeObservacion(obs);
                FormCollection form = new FormCollection();
                form.Add("operacion", idOp.ToString());
                return AccionOperaciones(idProy, form);
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost, Route("AccionRecorrido")]
        public ActionResult AccionRecorrido(int idProy, int idOp, int idObs, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];
                switch (accion)
                {
                    case "Cargar":
                        {
                            var ronda = int.Parse(collection["ronda"]);
                            return Detalles(idProy, idOp, idObs, ronda);
                        }
                    case "Agregar":
                        {
                            return Crear(idProy, idOp, idObs);
                        }
                    case "Eliminar":
                        {
                            var ronda = int.Parse(collection["ronda"]);
                            DatosRonda.Eliminar(ronda);

                            var form = new FormCollection();
                            form.Add("observacion", idObs.ToString());

                            return AccionObservacion(idProy, idOp, form);
                        }
                    case "Modificar":
                        {
                            var ronda = int.Parse(collection["ronda"]);
                            return Modificar(idProy, idOp, ronda, idObs);
                        }
                    default:
                        {
                            throw new Exception("No se ha reconocido la acción por realizar");
                        }
                }
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [HttpPost, Route("AccionObservacionTarea")]
        public ActionResult AccionObservacionTarea(int idProy, int idOp, int idObs, int idRonda, FormCollection collection)
        {
            try
            {
                var accion = collection["accion"];

                if (accion.Equals("Agregar"))
                {
                    var listaTemp = collection["valor"].Split(',');
                    var idSujeto = int.Parse(listaTemp[0]);
                    var idTarea = int.Parse(listaTemp[1]);
                    DatosObservacionTarea.Crear(idRonda, idSujeto, idTarea);
                }
                else
                {
                    var idObsTarea = int.Parse(collection["valor"]);
                    DatosObservacionTarea.Eliminar(idObsTarea);
                }

                FormCollection form = new FormCollection();
                form.Add("ronda", idRonda.ToString());

                return AccionRecorrido(idProy, idOp, idObs, form);
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Ronda/Crear
        [Route("Crear")]
        public ActionResult Crear(int idProy, int idOp, int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                ViewData["id_proyecto"] = idProy;
                ViewData["id_op"] = idOp;
                return View("Crear");
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Ronda/Crear
        [HttpPost, Route("Crear")]
        public ActionResult Crear(int idProy, int idOp, int idObservacion, FormCollection collection)
        {
            try
            {
                var hora = TimeSpan.Parse(collection["fecha_hora"]);
                var idObs = int.Parse(collection["id_observacion"]);
                var descripcion = collection["descripcion"];
                var humedad = float.Parse(collection["humedad"]);
                var temperatura = float.Parse(collection["temperatura"]);

                var res = DatosRonda.Crear(idObs, hora, humedad, temperatura, descripcion);

                return Detalles(idProy,idOp,idObservacion,res);
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        [Route("Detalles")]
        public ActionResult Detalles(int idProy, int idOp, int idObs, int idRonda)
        {
            try
            {
                var dic = DatosRonda.GetRonda(idRonda);

                ViewData["recorrido_sel"] = idRonda;
                ViewData["recorrido"] = dic;
                ViewData["obs_tarea"] = DatosObservacionTarea.GetObservacionTareaPorRecorrido(idRonda);
                ViewData["colaboradores"] = DatosSujeto.GetSujetosDeProyecto(idProy);
                ViewData["tareas"] = DatosTarea.getTareasDeActividad(idOp);

                var form = new FormCollection();
                form.Add("observacion", idObs.ToString());
                return AccionObservacion(idProy, idOp, form);
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Ronda/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int idProy, int idOp, int idRonda, int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                ViewData["ronda"] = DatosRonda.GetRonda(idRonda);
                ViewData["id_proyecto"] = idProy;
                ViewData["id_op"] = idOp;
                return View("Modificar");
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Ronda/Modificar/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idProy, int idOp, int idRonda, int idObservacion, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var fecha_hora = TimeSpan.Parse(collection["fecha_hora"]);
                var idObs = int.Parse(collection["id_observacion"]);
                var descripcion = collection["descripcion"];
                var humedad = float.Parse(collection["humedad"]);
                var temperatura = float.Parse(collection["temperatura"]);
                var idRon = int.Parse(collection["id_ronda"]);

                DatosRonda.Modificar(idRon, idObs, fecha_hora, humedad, temperatura, descripcion);

                return Detalles(idProy, idOp, idObservacion, idRon);
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View();
            }
        }

        // GET: Ronda/Delete/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int idRonda, int idObservacion)
        {
            try
            {
                DatosRonda.Eliminar(idRonda);
                return RedirectToAction("Index", new { idObservacion });
            }
            catch
            {
                ViewData["exception"] = new Exception(
                    "No se ha podido eliminar la ronda de observación");
                return View();
            }
        }
        
    }
}
