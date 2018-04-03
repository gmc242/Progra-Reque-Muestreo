using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    public class ObservacionTareasController : Controller
    {
        // GET: ObservacionTareas
        public ActionResult Index()
        {
            return View();
        }

        // GET: ObservacionTareas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ObservacionTareas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ObservacionTareas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        // GET: ObservacionTareas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ObservacionTareas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

        // GET: ObservacionTareas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ObservacionTareas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
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
