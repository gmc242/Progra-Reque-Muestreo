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
                    float tp = 0;
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
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareaXoperacion2", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@fecha", fecha));
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", ope));
                da.Fill(dt);
                var list = new List<Tuple<string, string, string>>();

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string obs = dt.Rows[i]["CantObs"].ToString();
                        

                        list.Add(new Tuple<string, string, string>(categoria, nombre, obs));
                        i++;
                    }
                    ViewData["TareasXoperacion"] = list;
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }


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

        private void tablaDatosResumen(string operacion)
        {
            var list = new List<Tuple<string, string>>();
            var productivas = new List<string>();
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getObservacionesXdiaXoperacion", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string fecha = truncarStr(10, dt.Rows[i]["Dia"].ToString());
                        string numero = dt.Rows[i]["Numero de Observaciones"].ToString();

                        list.Add(new Tuple<string, string>(fecha, numero));
                        i++;
                    }
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("productivasXoperacion", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string produc = dt.Rows[i]["Productivas"].ToString();

                        productivas.Add(produc);
                        i++;
                    }
                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }

            var result = new List<Tuple<string, string, string, string, string>>();
            float n = 0;

            for (int j = 0; j < list.Count; j++)
            {
                try
                {
                    float obs = Int32.Parse(list.ElementAt(j).Item2);
                    float tp = Int32.Parse(productivas.ElementAt(j));
                    float p = tp / obs;
                    float q = 1 - p;

                    result.Add(new Tuple<string, string, string, string, string>(list.ElementAt(j).Item1, (j+1).ToString(),
                        p.ToString(), q.ToString(), obs.ToString()));
                    n = n + obs;
                }
                catch { }
            }

            ViewData["TablaResumen"] = result;
            ViewData["nResumen"] = n;
        }

        private void tareasGeneral(string operacion)
        {
            var tareasGeneral = new List<Tuple<string, string, string>>();
            var tareasResumen = new List<Tuple<string, string, string, string>>();
            float tpg = 0;
            float tcg = 0;
            float tig = 0;
            float total = 0;
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareaXoperacion3", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string obs = dt.Rows[i]["CantObs"].ToString();

                        if (categoria == "TP")
                        {
                            float temp = Int32.Parse(obs);
                            tpg += temp;
                        }
                        else
                        {
                            if (categoria == "TC")
                            {
                                float temp = Int32.Parse(obs);
                                tcg += temp;
                            }
                            else
                            {
                                float temp = Int32.Parse(obs);
                                tig += temp;
                            }
                        }

                        tareasGeneral.Add(new Tuple<string, string, string>(categoria, nombre, obs));
                        i++;
                    }
                    total = tpg + tcg + tig;
                    float porP = (tpg / total) * 100;
                    float porC = (tcg / total) * 100;
                    float porI = (tig / total) * 100;
                    tareasResumen.Add(new Tuple<string, string, string, string>("TP", "Tareas Productivas", tpg.ToString(), porP.ToString()));
                    tareasResumen.Add(new Tuple<string, string, string, string>("TC", "Tareas Colaborativas", tcg.ToString(), porC.ToString()));
                    tareasResumen.Add(new Tuple<string, string, string, string>("TI", "Tareas Improductivas", tig.ToString(), porI.ToString()));

                    ViewData["TareasGeneral"] = tareasGeneral;
                    ViewData["TareasResumen"] = tareasResumen;
                    ViewData["totalGeneral"] = total;

                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
        }

        private void tareasProductivas(string operacion)
        {
            var tareasProductivas = new List<Tuple<string, string, string>>();
            float total = 0;
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareaXoperacionTP", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string obs = dt.Rows[i]["CantObs"].ToString();
                        
                        float temp = Int32.Parse(obs);
                        total += temp;

                        tareasProductivas.Add(new Tuple<string, string, string>(categoria, nombre, obs));
                        i++;
                    }
                    ViewData["TareasProductivas"] = tareasProductivas;
                    ViewData["totalProductivas"] = total;

                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
        }

        private void tareasColaborativas(string operacion)
        {
            var tareasColaborativas = new List<Tuple<string, string, string>>();
            float total = 0;
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareaXoperacionTC", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string obs = dt.Rows[i]["CantObs"].ToString();

                        float temp = Int32.Parse(obs);
                        total += temp;

                        tareasColaborativas.Add(new Tuple<string, string, string>(categoria, nombre, obs));
                        i++;
                    }
                    ViewData["TareasColaborativas"] = tareasColaborativas;
                    ViewData["totalColaborativas"] = total;

                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
        }

        private void tareasImproductivas(string operacion)
        {
            var tareasImproductivas = new List<Tuple<string, string, string>>();
            float total = 0;
            using (var conn = ControladorGlobal.GetConn())
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter("getTareaXoperacionTI", conn);
                conn.Open();

                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.Add(new SqlParameter("@nombreOperacion", operacion));
                da.Fill(dt);

                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    int i = 0;
                    while (i < rows)
                    {
                        string categoria = dt.Rows[i]["Categoria"].ToString();
                        string nombre = dt.Rows[i]["Nombre"].ToString();
                        string obs = dt.Rows[i]["CantObs"].ToString();

                        float temp = Int32.Parse(obs);
                        total += temp;

                        tareasImproductivas.Add(new Tuple<string, string, string>(categoria, nombre, obs));
                        i++;
                    }
                    ViewData["TareasImproductivas"] = tareasImproductivas;
                    ViewData["totalImproductivas"] = total;

                }
                else
                {
                    ViewBag.Msj = "No hay operaciones agregadas";
                }
            }
        }

        public ActionResult Consolidado(FormCollection collection)
        {
            string operacion = collection["operacion"].ToString();
            ViewBag.Titulo = "Consolidación "+operacion;

            //----------------------Tabla Datos Resumen--------------------------------------------
            tablaDatosResumen(operacion);
            //---------------------------Tareas General por Operacion-----------------------------
            tareasGeneral(operacion);
            //---------------------Tareas Productivas---------------
            tareasProductivas(operacion);
            //---------------------Tareas Colaborativas---------------
            tareasColaborativas(operacion);
            //---------------------Tareas Imroductivas---------------
            tareasImproductivas(operacion);


            return View();
        }

    }
}
