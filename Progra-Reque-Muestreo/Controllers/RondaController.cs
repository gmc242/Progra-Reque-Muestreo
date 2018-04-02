using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Actividad/Ronda")]
    [Route("{action}")]
    public class RondaController : Controller
    {
        // GET: Ronda
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }

        // GET: Ronda/Crear
        [Route("Crear")]
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Ronda/Crear
        [HttpPost, Route("Crear")]
        public ActionResult Crear(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ronda/Modificar/5
        [Route("Modificar")]
        public ActionResult Modificar(int id)
        {
            return View();
        }

        // POST: Ronda/Modificar/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Ronda/Delete/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int id)
        {
            return View();
        }

        // POST: Ronda/Delete/5
        [HttpPost, Route("Eliminar")]
        public ActionResult Eliminar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
