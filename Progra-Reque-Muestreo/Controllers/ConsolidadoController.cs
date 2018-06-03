using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Web.Helpers;
using System.Web.DataAccess;

namespace Progra_Reque_Muestreo.Views.Consolidado
{
    public class ConsolidadoController : Controller
    {
        private void getOperations()
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getOperaciones", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.Fill(dt);
                var lista = new List<string>();

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string operacion = dt.Rows[i]["nombre"].ToString();
                        lista.Add(operacion);
                        i++;


                    }
                    ViewBag.len = rows;
                    ViewData["Operaciones"] = lista;
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
        }
        public ActionResult TipoConsolidado()
        {
            getOperations();
            return View();
        }
        public ActionResult TipoFecha()
        {
            getOperations();
            return View();
        }

        public ActionResult DatosXFecha(FormCollection collection)
        {
            string operacion = collection["operacion"].ToString();
            ViewBag.Titulo = "Datos por Fecha de "+operacion;
            return View();
        }
        public ActionResult Consolidado(FormCollection collection)
        {
            string operacion = collection["operacion"].ToString();
            ViewBag.Titulo = operacion;

            //using (var conn = ControladorGlobal.GetConn())
            //{
            //    DataTable dt = new DataTable();
            //    SqlDataAdapter da = new SqlDataAdapter("getOperaciones", conn);
            //    conn.Open();

            //    da.SelectCommand.CommandType = CommandType.StoredProcedure;
            //    da.Fill(dt);
            //    var lista = new List<string>();

            //    int rows = dt.Rows.Count;
            //    if (rows > 0)
            //    {
            //        int i = 0;
            //        while (i < rows)
            //        {
            //            //string operacion = dt.Rows[i]["nombre"].ToString();
            //            //lista.Add(operacion);
            //            //i++;


            //        }
            //        ViewBag.len = rows;
            //        ViewData["Operaciones"] = lista;
            //    }
            //    else
            //    {
            //        ViewBag.Msj = "No hay Observaciones para "+operacion;
            //    }
            //}


            return View();
        }

        public ActionResult VerDatos()
        {
            return View();
        }

    }
}
