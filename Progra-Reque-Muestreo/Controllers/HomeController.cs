using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Web.DataAccess;

namespace Progra_Reque_Muestreo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //int c = 5;
            //for (int i =0; i<c; i++)
            //{
            //    ViewData.Add("a"+i, "Observacion"+i);

            //}
            //ViewBag.Lengh = 5;
            //ViewData.Add("Tarea", "Observaciones");
            //ViewData.Add("Productivas", 90);
            //ViewData.Add("Colaborativas", 46);
            //ViewData.Add("Improductivas", 73);
            ViewBag.Msj = getData();
            return View();
        }

        protected string getData()
        {
            string cadena = "[['Tarea', 'Observaciones'],['Productivas', 90],['Contributivas', 46],['Improductivas', 73]]";
            return cadena;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}