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
        static string op;
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

        public string truncarStr(int n, string str)
        {
            int i = 0;
            string result = "";
            foreach (char c in str)
            {
                if (i < n)
                {
                    result = result + c;
                    i++;
                }
            }
            return result;
        }

        public string crearFecha(string str)
        {
            int flag = 0;
            string dia = "";
            string mes = "";
            string anio = "";

            string result = "";
            foreach (char c in str)
            {
                if (flag == 0)
                {
                    if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '0')
                    {
                        dia += c;
                    }
                    else
                    {
                        flag++;
                    }
                }
                else
                {
                    if (flag == 1)
                    {
                        if (c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == '0')
                        {
                            mes += c;
                        }
                        else
                        {
                            flag++;
                        }
                    }
                    else
                    {
                        anio += c;
                    }
                }
            }
            result = anio + "-" + mes + "-" + dia;
            return result;
        }
        public ActionResult DatosXFecha(FormCollection collection)
        {
            string f = collection["fecha"].ToString();
            string fecha = crearFecha(f);
            ViewBag.Titulo = op+" "+fecha;
            string ope = op;

            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareasXoperacion", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@fecha", fecha));
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", ope));
                da.Fill(dt);
                var listatareas = new List<Tuple<string,string,string>>();
                var listaHHT = new List<Tuple<string, string, string>>();

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    float h = 0;
                    float t = 0;
                    int tp = 0;
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string sujeto = dt.Rows[i]["Colaborador"].ToString();
                        string hora = truncarStr(8,dt.Rows[i]["Hora"].ToString());
                        string humedad = dt.Rows[i]["Humedad"].ToString();
                        string tempe = dt.Rows[i]["Temperatura"].ToString();
                        try
                        {
                            h += Int32.Parse(humedad);
                            t += Int32.Parse(tempe);
                        }
                        catch{}

                        listatareas.Add(new Tuple<string, string,string>(categoria,nombre,sujeto));
                        listaHHT.Add(new Tuple<string, string, string>(hora, humedad, tempe));
                        i++;
                        if (categoria == "TP")
                        {
                            tp++;
                        }

                    }

                    float p = 0;
                    p = tp / rows;
                    float q = 1 - p;
                    h = h / rows;
                    t = t / rows;
                    ViewData["ResultadoTareas"] = listatareas;
                    ViewData["HHT"] = listaHHT;
                    ViewData["n"] = rows;
                    ViewData["p"] = p;
                    ViewData["q"] = q;
                    ViewData["humedad"] = h;
                    ViewData["temperatura"] = t;
                    ViewData["TP"] = tp; 
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }


            return View();
        }
        public ActionResult Consolidado(FormCollection collection)
        {
            string operacion = collection["operacion"].ToString();
            ViewBag.Titulo = operacion;
            return View();
        }
        public ActionResult SeleccionarFecha(FormCollection collection)
        {
            
            string operacion = collection["operacion"].ToString();
            ViewBag.Titulo = "Datos por Fecha de " + operacion;
            op = operacion;

            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getFechas", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);
                var lista = new List<string>();

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string fecha = truncarStr(10, dt.Rows[i]["dia"].ToString());
                        lista.Add(fecha);
                        i++;


                    }
                    ViewBag.len = rows;
                    ViewData["Fechas"] = lista;
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
            return View();
        }


        public ActionResult VerDatos()
        {
            return View();
        }

    }
}
