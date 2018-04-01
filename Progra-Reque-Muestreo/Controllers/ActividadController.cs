using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Actividad")]
    [Route("{action}")]
    public class ActividadController : Controller
    {
        // GET: Proyecto/Actividad
        [Route("Index")]
        public ActionResult Index(String idProyecto)
        {
            var proyecto = DatosProyecto.GetProyecto(int.Parse(idProyecto));
            ViewData["nombre"] = proyecto["nombre"];
            ViewData["id"] = idProyecto;

            List<Tuple<int, String>> lista = DatosActividad.getActividades(int.Parse(idProyecto));
            ViewData["actividades"] = lista;

            return View();
        }

        // GET: Actividad/Crear
        [Route("Crear")]
        public ActionResult Crear(String id)
        {
            ViewData["idProyecto"] = id;
            ViewData["usuarios"] = DatosActividad.getUsuariosParaProyecto(int.Parse(id));
            return View();
        }

        // POST: Actividad/Crear
        [Route("Crear"), HttpPost]
        public ActionResult Crear(String idProyecto, FormCollection collection)
        {
            try
            {
                // nombre, descripcion, usuarios
                var nombre = collection["nombre"].ToString();
                var descripcion = collection["descripcion"].ToString();
                String[] usuarios = null;

                if (collection.AllKeys.Contains("usuarios[]"))
                {
                    usuarios = collection["usuarios[]"].ToString().Split(',');
                }

                DatosActividad.CrearActividad(int.Parse(idProyecto), nombre, descripcion, usuarios);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Actividad/Modificar
        [Route("Modificar")]
        public ActionResult Modificar(String idActividad)
        {
            var dic = DatosActividad.getActividad(int.Parse(idActividad));
            ViewData["usuarios"] = DatosActividad.getUsuariosParaProyecto(dic["id_proyecto"]);
            ViewData["actividad"] = dic;

            return View();
        }

        // POST: Actividad/Modificar
        [Route("Modificar"), HttpPost]
        public ActionResult Modificar(String idActividad, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var descripcion = collection["descripcion"].ToString();
                var idProyecto = int.Parse(collection["idProyecto"].ToString());
                String[] usuarios = null;

                if (collection.AllKeys.Contains("usuarios[]"))
                {
                    usuarios = collection["usuarios[]"].ToString().Split(',');
                }

                DatosActividad.ModificarActividad(int.Parse(idActividad), idProyecto, nombre, descripcion, usuarios);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Actividad/Delete/5
        [Route("Eliminar")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Actividad/Delete/5
        [Route("Eliminar"), HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }
    }
}
