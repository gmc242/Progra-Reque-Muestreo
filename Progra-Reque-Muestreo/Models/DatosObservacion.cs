using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosObservacion
    {
        public static List<Tuple<int,String>> GetObservacionesPorProyecto(int idProyecto)
        {
            var lista = new List<Tuple<int, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT a.nombre, o.fecha, o.id_observacion FROM observacion AS o " +
                    "INNER JOIN actividad AS a ON a.id_actividad = o.id_actividad WHERE a.id_proyecto = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idProyecto };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id_observacion = reader["id_observacion"];
                        String s = "Observación de ID " + id_observacion.ToString() +
                            " sobre la actividad " + reader["nombre"].ToString() + 
                            " el dia " + ((DateTime)reader["dia"]).ToString(ControladorGlobal.GetDateFormat());
                        lista.Add(new Tuple<int, string>((int)reader["id_observacion"], s));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static Dictionary<String, dynamic> GetObservacion(int idObservacion)
        {
            var dic = new Dictionary<String, dynamic>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT o.id_observacion, o.descripcion, o.dia , a.nombre, a.id_actividad, p.nombre, p.id_proyecto" +
                    "FROM observacion AS o INNER JOIN " +
                    "(SELECT a.nombre, a.id_actividad, p_nombre, p.id_proyecto FROM actividad AS a " +
                    "INNER JOIN proyecto AS p ON a.id_proyecto = p.id_proyecto) " +
                    "ON o.id_actividad = a.actividad WHERE o.id_observacion = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacion };

                command.Parameters.Add(idP);
                command.Prepare();

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["id_observacion"] = (int)reader["o.id_observacion"];
                        dic["descripcion"] = reader["o.descripcion"].ToString();
                        dic["dia"] = (DateTime)reader["o.dia"];
                        dic["nombre_actividad"] = reader["a.nombre"].ToString();
                        dic["id_actividad"] = (int)reader["a.id_actividad"];
                        dic["nombre_proyecto"] = reader["p.nombre"].ToString();
                        dic["id_proyecto"] = (int)reader["p.id_proyecto"];
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static int Crear(int idActividad, String descripcion, DateTime dia)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "INSERT INTO observacion(id_actividad, descripcion, dia) " +
                    "OUTPUT Inserted.id_observacion VALUES(@id_actividad, @descripcion, @dia", conn);

                var idA = new SqlParameter("@id_actividad", SqlDbType.Int, 0) { Value = idActividad };
                var desc = new SqlParameter("@descripcion", SqlDbType.VarChar, 200) { Value = descripcion };
                var diaP = new SqlParameter("@dia", SqlDbType.Date, 0) { Value = dia };

                command.Parameters.Add(idA);
                command.Parameters.Add(desc);
                command.Parameters.Add(diaP);

                command.Prepare();
                var res = (int)command.ExecuteScalar();

                conn.Close();

                return res;
            }
        }

        public static void Modificar(int idObservacion, int idActividad, String descripcion, DateTime dia)
        {
            using(var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "UPDATE observacion SET id_actividad = @id_actividad, descripcion = @descripcion, " +
                    "dia = @dia WHERE id_observacion = @id_observacion", conn);

                var idA = new SqlParameter("@id_actividad", SqlDbType.Int, 0) { Value = idActividad };
                var idO = new SqlParameter("@id_observacion", SqlDbType.Int, 0) { Value = idObservacion };
                var desc = new SqlParameter("@descripcion", SqlDbType.VarChar, 200) { Value = descripcion };
                var diaP = new SqlParameter("@dia", SqlDbType.Date, 0) { Value = dia };

                command.Parameters.Add(idA);
                command.Parameters.Add(idO);
                command.Parameters.Add(desc);
                command.Parameters.Add(diaP);

                command.Prepare();

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void Eliminar(int idObservacion)
        {
            using(var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM observacion WHERE id_observacion = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacion };

                command.Parameters.Add(idP);
                command.Prepare();
                command.ExecuteNonQuery();

                conn.Close();
            }
        }

    }
}