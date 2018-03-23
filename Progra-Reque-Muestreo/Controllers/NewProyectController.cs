using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    public class NewProyectController : Controller
    {
        // GET: NewProyect
        public ActionResult Index()
        {
            return View();
        }

        // GET: NewProyect/Create
        public ActionResult Create()
        {
            var ejemplosUsuarios = new List<String>();
            ejemplosUsuarios.Add("Nombre 1. Id: 1");
            ejemplosUsuarios.Add("Nombre 1. Id: 2");
            ejemplosUsuarios.Add("Nombre 1. Id: 3");
            ViewData["nombresUsuarios"] = ejemplosUsuarios;
            return View();
        }

        // POST: NewProyect/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                // Acá llega el collection con la información colectada en el form
                // Se itera o revisa de forma collection["nombre"]
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
