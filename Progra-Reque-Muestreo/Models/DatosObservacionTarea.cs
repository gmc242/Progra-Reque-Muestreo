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
                    "SELECT ro.id_observacion, ro.id_ronda, temp_outer.categoria, temp_outer.nombre_sujeto, " +
                    "temp_outer.nombre_tarea, temp_outer.id_tarea, temp_outer.id_sujeto, temp_outer.id_observacion_tarea " +
                    "FROM ronda_de_observacion ro INNER JOIN " +
                    "( " +
                    "SELECT t.categoria, t.nombre AS nombre_tarea, temp.nombre_sujeto, temp.id_ronda, t.id_tarea, temp.id_sujeto, temp.id_observacion_tarea " +
                    "FROM tarea t INNER JOIN " +
                    "( " +
                    "SELECT s.id_sujeto, id_ronda, s.nombre AS nombre_sujeto, oa.id_tarea, oa.id_observacion_tarea FROM sujetos_de_prueba s " +
                    "INNER JOIN observacion_de_tarea oa " +
                    "ON s.id_sujeto = oa.id_sujeto " +
                    ") AS temp " +
                    "ON t.id_tarea = temp.id_tarea " +
                    ") AS temp_outer " +
                    "ON temp_outer.id_ronda = ro.id_ronda " +
                    "WHERE ro.id_observacion = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacion };
                command.Parameters.Add(idP);
                command.Prepare();

                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dic = new Dictionary<String, dynamic>();
                        dic["id_observacion"] = (int)reader["id_observacion"];
                        dic["ronda"] = (int)reader["id_ronda"];
                        dic["categoria"] = reader["categoria"].ToString();
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
                    "SELECT ro.id_observacion, ro.id_ronda, temp_outer.categoria, temp_outer.nombre_sujeto, " +
                    "temp_outer.nombre_tarea, temp_outer.id_tarea, temp_outer.id_sujeto, temp_outer.id_observacion_tarea " +
                    "FROM ronda_de_observacion ro INNER JOIN " +
                    "( " +
                    "SELECT t.categoria, t.nombre AS nombre_tarea, temp.nombre_sujeto, temp.id_ronda, t.id_tarea, temp.id_sujeto, temp.id_observacion_tarea " +
                    "FROM tarea t INNER JOIN " +
                    "( " +
                    "SELECT s.id_sujeto, id_ronda, s.nombre AS nombre_sujeto, oa.id_tarea, oa.id_observacion_tarea FROM sujetos_de_prueba s " +
                    "INNER JOIN observacion_de_tarea oa " +
                    "ON s.id_sujeto = oa.id_sujeto " +
                    ") AS temp " +
                    "ON t.id_tarea = temp.id_tarea " +
                    ") AS temp_outer " +
                    "ON temp_outer.id_ronda = ro.id_ronda " +
                    "WHERE temp_outer.id_observacion_tarea = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacionTarea };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["id_observacion"] = (int)reader["id_observacion"];
                        dic["ronda"] = (int)reader["id_ronda"];
                        dic["categoria"] = reader["categoria"].ToString();
                        dic["id_observacion_tarea"] = (int)reader["id_observacion_tarea"];
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

        public static List<Dictionary<String, dynamic>> GetObservacionTareaPorRecorrido(int idRecorrido)
        {
            var lista = new List<Dictionary<String, dynamic>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT ro.id_observacion, ro.id_ronda, temp_outer.categoria, temp_outer.nombre_sujeto, " +
                    "temp_outer.nombre_tarea, temp_outer.id_tarea, temp_outer.id_sujeto, temp_outer.id_observacion_tarea " +
                    "FROM ronda_de_observacion ro INNER JOIN " +
                    "( " +
                    "SELECT t.categoria, t.nombre AS nombre_tarea, temp.nombre_sujeto, temp.id_ronda, t.id_tarea, temp.id_sujeto, temp.id_observacion_tarea " +
                    "FROM tarea t INNER JOIN " +
                    "( " +
                    "SELECT s.id_sujeto, id_ronda, s.nombre AS nombre_sujeto, oa.id_tarea, oa.id_observacion_tarea FROM sujetos_de_prueba s " +
                    "INNER JOIN observacion_de_tarea oa " +
                    "ON s.id_sujeto = oa.id_sujeto " +
                    ") AS temp " +
                    "ON t.id_tarea = temp.id_tarea " +
                    ") AS temp_outer " +
                    "ON temp_outer.id_ronda = ro.id_ronda " +
                    "WHERE ro.id_ronda = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idRecorrido };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dic = new Dictionary<String, dynamic>();
                        dic["id_observacion"] = (int)reader["id_observacion"];
                        dic["ronda"] = (int)reader["id_ronda"];
                        dic["categoria"] = reader["categoria"].ToString();
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

        public static List<Dictionary<String, dynamic>> GetObservacionTareaPorOperacion(int idOperacion)
        {
            var lista = new List<Dictionary<String, dynamic>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT ro.id_observacion, ro.id_ronda, temp_outer.categoria, temp_outer.nombre_sujeto, " +
                    "temp_outer.nombre_tarea, temp_outer.id_tarea, temp_outer.id_sujeto, temp_outer.id_observacion_tarea, temp_outer.id_actividad " +
                    "FROM ronda_de_observacion ro INNER JOIN " +
                    "( " +
                    "SELECT t.categoria, t.nombre AS nombre_tarea, temp.nombre_sujeto, temp.id_ronda, t.id_tarea, temp.id_sujeto, temp.id_observacion_tarea, t.id_actividad " +
                    "FROM tarea t INNER JOIN " +
                    "( " +
                    "SELECT s.id_sujeto, id_ronda, s.nombre AS nombre_sujeto, oa.id_tarea, oa.id_observacion_tarea FROM sujetos_de_prueba s " +
                    "INNER JOIN observacion_de_tarea oa " +
                    "ON s.id_sujeto = oa.id_sujeto " +
                    ") AS temp " +
                    "ON t.id_tarea = temp.id_tarea " +
                    ") AS temp_outer " +
                    "ON temp_outer.id_ronda = ro.id_ronda " +
                    "WHERE temp_outer.id_actividad = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idOperacion };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dic = new Dictionary<String, dynamic>();
                        dic["id_actividad"] = (int)reader["id_actividad"];
                        dic["id_observacion"] = (int)reader["id_observacion"];
                        dic["ronda"] = (int)reader["id_ronda"];
                        dic["categoria"] = reader["categoria"].ToString();
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

        public static int GetCantidadObservacionesPorOperacion(int idOperacion)
        {
            int res = 0;

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT temp_outer.id_actividad, count(id_observacion) as cantidad " +
                    "FROM ronda_de_observacion ro INNER JOIN " +
                    "( " +
                    "SELECT t.categoria, t.nombre AS nombre_tarea, temp.nombre_sujeto, temp.id_ronda, t.id_tarea, temp.id_sujeto, temp.id_observacion_tarea, t.id_actividad " +
                    "FROM tarea t INNER JOIN " +
                    "( " +
                    "SELECT s.id_sujeto, id_ronda, s.nombre AS nombre_sujeto, oa.id_tarea, oa.id_observacion_tarea FROM sujetos_de_prueba s " +
                    "INNER JOIN observacion_de_tarea oa " +
                    "ON s.id_sujeto = oa.id_sujeto " +
                    ") AS temp " +
                    "ON t.id_tarea = temp.id_tarea " +
                    ") AS temp_outer " +
                    "ON temp_outer.id_ronda = ro.id_ronda " +
                    "WHERE temp_outer.id_actividad = @id " +
                    "GROUP BY id_actividad", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idOperacion };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        res = (int)reader["cantidad"];
                    }
                }

                conn.Close();
            }

            return res;
        }

        public static int Crear(int idRonda, int idSujeto, int idTarea)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand
                    ("INSERT INTO observacion_de_tarea(id_ronda, id_sujeto, id_tarea) " +
                    "OUTPUT Inserted.id_observacion_tarea VALUES(@id_ron, @id_s, @id_t)", conn);

                var idRon = new SqlParameter("@id_ron", SqlDbType.Int, 0) { Value = idRonda };
                var idS = new SqlParameter("@id_s", SqlDbType.Int, 0) { Value = idSujeto };
                var idT = new SqlParameter("@id_t", SqlDbType.Int, 0) { Value = idTarea };

                command.Parameters.Add(idRon);
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