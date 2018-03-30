using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    public class NewProyectController : Controller
    {
        // GET: NewProyect
        public ActionResult Index() => View();

        // GET: NewProyect/Create
        public ActionResult Create()
        {
            var usuarios = DatosUsuarios.getUsuariosString();
            ViewData["usuarios"] = usuarios;
            return View();
        }

        // POST: NewProyect/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            // TODO: Add insert logic here
            // Acá llega el collection con la información colectada en el form
            try
            {
                // Parsing de datos de la forma HTML
                String nombre = collection["nombre"];
                int tiempoObs = int.Parse(collection["tiempoObservacion"]);
                var inicio = DateTime.Parse(collection["inicio"]);
                var final = DateTime.Parse(collection["final"]);
                int tamano = int.Parse(collection["tamano"]);
                int tiempoEntreObservaciones = int.Parse(collection["tiempoEntreObservaciones"]);
                String idLider = collection["lider"];
                String descripcion = collection["descripcion"];

                String[] idAsistentesS = collection["asistentes[]"].Split(',');

                this.agregarProyecto(nombre, tiempoObs, inicio, final, tamano, tiempoEntreObservaciones,
                    idLider, descripcion);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewData["Exception"] = e;
                return View("Error");
            }
        }

        private void agregarProyecto(String nombre, int tiempoObs, DateTime inicio, DateTime final,
        int tamano, int tiempoEntreObservaciones, String idLider, String descripcion)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var stmn = new SqlCommand(
                    "INSERT INTO proyecto(nombre, tiempo_muestreo, tamano_muestreo, tiempo_entre_muestreos, " +
                    "fecha_inicio, fecha_fin, lider_id, descripcion) VALUES (@nom, @tiempo_muestreo, " +
                    "@tamano_muestreo, @tiempo_entre_muestreos, @fecha_inicio, @fecha_fin, @lider_id, @descripcion)",
                    conn);

                var nom = new SqlParameter("@nom", SqlDbType.VarChar, 50);
                var tiempo_muestreo = new SqlParameter("@tiempo_muestreo", SqlDbType.Int, 0);
                var tamano_muestreo = new SqlParameter("@tamano_muestreo", SqlDbType.Int, 0);
                var tiempo_entre_muestreos = new SqlParameter("@tiempo_entre_muestreos", SqlDbType.Int, 0);
                var fecha_inicio = new SqlParameter("@fecha_inicio", SqlDbType.Date, 0);
                var fecha_final = new SqlParameter("@fecha_fin", SqlDbType.Date, 0);
                var lider = new SqlParameter("@lider_id", SqlDbType.VarChar, 40);
                var desc = new SqlParameter("@descripcion", SqlDbType.VarChar, 1000);

                nom.Value = nombre;
                tiempo_muestreo.Value = tiempoObs;
                tiempo_entre_muestreos.Value = tiempoEntreObservaciones;
                tamano_muestreo.Value = tamano;
                fecha_final.Value = final;
                fecha_inicio.Value = inicio;
                desc.Value = descripcion;
                lider.Value = idLider;

                stmn.Parameters.Add(nom);
                stmn.Parameters.Add(tiempo_muestreo);
                stmn.Parameters.Add(tamano_muestreo);
                stmn.Parameters.Add(tiempo_entre_muestreos);
                stmn.Parameters.Add(fecha_inicio);
                stmn.Parameters.Add(fecha_final);
                stmn.Parameters.Add(lider);
                stmn.Parameters.Add(desc);

                stmn.Prepare();
                stmn.ExecuteNonQuery();
                conn.Close();

            }
        }
    }
}
