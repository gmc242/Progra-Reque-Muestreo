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
        public ActionResult Index(int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                ViewData["rondas"] = DatosRonda.GetRondasDeObservacion(idObservacion);
                return View();
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Ronda/Crear
        [Route("Crear")]
        public ActionResult Crear(int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                return View();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Ronda/Crear
        [HttpPost, Route("Crear")]
        public ActionResult Crear(int idObservacion, FormCollection collection)
        {
            try
            {
                var fecha_hora = DateTime.Parse(collection["fecha_hora"]);
                var idObs = int.Parse(collection["id_observacion"]);
                var descripcion = collection["descripcion"];
                var humedad = float.Parse(collection["humedad"]);
                var temperatura = float.Parse(collection["temperatura"]);

                DatosRonda.Crear(idObs, fecha_hora, humedad, temperatura, descripcion);

                return RedirectToAction("Index", new { idObservacion });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Ronda/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int idRonda, int idObservacion)
        {
            try
            {
                var dic_obs = DatosObservacion.GetObservacion(idObservacion);
                ViewData["observacion"] = DatosObservacion.ToTuple(dic_obs);
                ViewData["ronda"] = DatosRonda.GetRonda(idRonda);
                return View();
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Ronda/Modificar/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idRonda, int idObservacion, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                var fecha_hora = DateTime.Parse(collection["fecha_hora"]);
                var idObs = int.Parse(collection["id_observacion"]);
                var descripcion = collection["descripcion"];
                var humedad = float.Parse(collection["humedad"]);
                var temperatura = float.Parse(collection["temperatura"]);
                var idRon = int.Parse(collection["id_ronda"]);

                DatosRonda.Modificar(idRon, idObs, fecha_hora, humedad, temperatura, descripcion);

                return RedirectToAction("Index", new { idObservacion });
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
