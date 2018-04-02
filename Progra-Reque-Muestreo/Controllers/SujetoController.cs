using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    [RoutePrefix("Proyecto/Sujeto")]
    [Route("{action}")]
    public class SujetoController : Controller
    {
        // GET: Sujeto
        [Route("Index")]
        public ActionResult Index(int idProyecto)
        {
            var dic = DatosProyecto.GetProyecto(idProyecto);
            ViewData["proyecto"] = new Tuple<int, String>(dic["id_proyecto"], dic["nombre"]);
            ViewData["sujetos"] = DatosSujeto.GetSujetosDeProyecto(idProyecto);
            return View();
        }

        // POST: Sujeto/Create
        [HttpPost, Route("Crear")]
        public ActionResult Crear(int idProyecto, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var idProy = int.Parse(collection["id_proyecto"]);

                DatosSujeto.Crear(nombre, idProy);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Sujeto/Modificar/5
        [HttpPost, Route("Modificar")]
        public ActionResult Modificar(int idSujeto, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var idProyecto = int.Parse(collection["id_proyecto"]);
                var idSuj = int.Parse(collection["id_sujeto"]);

                DatosSujeto.Modificar(idSuj, nombre, idProyecto);

                return RedirectToAction("Index", new { idProyecto });
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Sujeto/Eliminar/5
        [Route("Eliminar")]
        public ActionResult Eliminar(int idSujeto, int idProyecto)
        {
            try
            {
                DatosSujeto.Eliminar(idSujeto);
                return RedirectToAction("Index", new { idProyecto });
            }
            catch(Exception e)
            {
                ViewData["exception"] = new Exception("No se ha podido borrar el sujeto, revise " +
                    "y elimine primero los datos que dependen de este sujeto");
                return View("Error");
            }
        }

        
    }
}
