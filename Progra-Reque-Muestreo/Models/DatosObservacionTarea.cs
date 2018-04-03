using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosObservacionTarea
    {
        public static List<Dictionary<String, dynamic>> GetObservacionTareaPorObservacion(int idObservacion)
        {
            var lista = new List<Dictionary<String, dynamic>>();

            using(var conn= ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT id_observacion_tarea, id_sujeto, oas.nombre AS nombre_sujeto, t.id_tarea, t.nombre AS nombre_tarea " +
                    "FROM tarea AS t INNER JOIN " +
                    "(SELECT id_observacion_tarea, s.id_sujeto, nombre, id_tarea, id_observacion " +
                    "FROM observacion_de_tarea AS oa " +
                    "INNER JOIN sujetos_de_prueba AS s " +
                    "ON oa.id_sujeto = s.id_sujeto) AS oas " +
                    "ON oas.id_tarea = t.id_tarea WHERE oas.id_observacion = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacion };
                command.Parameters.Add(idP);
                command.Prepare();

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dic = new Dictionary<String, dynamic>();
                        dic["id_observacion_tarea"] = (int)reader["id_observacion_tarea"];
                        dic["id_sujeto"] = (int)reader["id_sujeto"];
                        dic["nombre_sujeto"] = reader["nombre_sujeto"].ToString();
                        dic["id_tarea"] = (int)reader["id_tarea"];
                        dic["nombre_tarea"] = reader["nombre_tarea"].ToString();
                        lista.Add(dic);
                    }
                }

                conn.Close();
            }

            return lista;
        } 

        public static Dictionary<String, dynamic> GetObservacionTarea(int idObservacionTarea)
        {
            var dic = new Dictionary<String, dynamic>();

            using(var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT id_observacion_tarea, id_sujeto, oas.nombre AS nombre_sujeto, t.id_tarea, t.nombre AS nombre_tarea " +
                    "FROM tarea AS t INNER JOIN " +
                    "(SELECT id_observacion_tarea, s.id_sujeto, nombre, id_tarea, id_observacion " +
                    "FROM observacion_de_tarea AS oa " +
                    "INNER JOIN sujetos_de_prueba AS s " +
                    "ON oa.id_sujeto = s.id_sujeto) AS oas " +
                    "ON oas.id_tarea = t.id_tarea WHERE oas.id_observacion_tarea = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacionTarea };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["id_observacion_tarea"] = (int)reader["id_observacion"];
                        dic["id_sujeto"] = (int)reader["id_sujeto"];
                        dic["nombre_sujeto"] = reader["nombre_sujeto"].ToString();
                        dic["id_tarea"] = (int)reader["id_tarea"];
                        dic["nombre_tarea"] = reader["nombre_tarea"].ToString();
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static int Crear(int idObservacion, int idSujeto, int idTarea)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand
                    ("INSERT INTO observacion_de_tarea(id_observacion, id_sujeto, id_tarea) " +
                    "OUTPUT Inserted.id_observacion_tarea VALUES(@id_obs, @id_s, @id_t)", conn);

                var idObs = new SqlParameter("@id_obs", SqlDbType.Int, 0) { Value = idObservacion };
                var idS = new SqlParameter("@id_s", SqlDbType.Int, 0) { Value = idSujeto };
                var idT = new SqlParameter("@id_t", SqlDbType.Int, 0) { Value = idTarea };

                command.Parameters.Add(idObs);
                command.Parameters.Add(idS);
                command.Parameters.Add(idT);

                command.Prepare();

                var res = (int)command.ExecuteScalar();

                conn.Close();

                return res;
            }
        }

        public static void Eliminar(int idObservacionTarea)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM observacion_de_tarea WHERE id_observacion_tarea = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacionTarea };
                command.Parameters.Add(idP);
                command.Prepare();
                command.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}