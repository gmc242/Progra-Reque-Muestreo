using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosRonda
    {
        public static List<Tuple<int, String>> GetRondasDeObservacion(int idObservacion)
        {
            var lista = new List<Tuple<int, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT nombre, r.id_observacion, id_ronda, fecha_hora FROM ronda_de_observacion AS r INNER JOIN " +
                    "(SELECT id_observacion, nombre FROM observacion AS o INNER JOIN actividad AS a ON o.id_actividad = a.id_actividad) AS ao " +
                    "ON r.id_observacion = ao.id_observacion WHERE r.id_observacion = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idObservacion };
                command.Parameters.Add(idP);
                command.Prepare();

                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        DateTime fecha = (DateTime)reader["fecha_hora"];
                        int idRonda = (int)reader["id_ronda"];

                        String s = "Ronda de Observación de ID: " + idRonda.ToString() +
                            " hecha el dia " + fecha.ToString("yyyy-MM-dd") + " a las horas: " + fecha.ToString("HH:mm") +
                            " sobre la observación de ID: " + reader["id_observacion"] + " de la actividad " + reader["nombre"];

                        lista.Add(new Tuple<int, String>(idRonda, s));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static Dictionary<String, dynamic> GetRonda(int idRonda)
        {
            var dic = new Dictionary<String, dynamic>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT nombre, r.id_observacion, id_ronda, fecha_hora, humedad, temperatura, r.descripcion FROM ronda_de_observacion AS r INNER JOIN " +
                    "(SELECT id_observacion, nombre FROM observacion AS o INNER JOIN actividad AS a ON o.id_actividad = a.id_actividad) AS ao " +
                    "ON r.id_observacion = ao.id_observacion WHERE id_ronda = @ronda", conn);

                var idP = new SqlParameter("@ronda", SqlDbType.Int, 0) { Value = idRonda };
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["id_ronda"] = (int)reader["id_ronda"];
                        dic["id_observacion"] = (int)reader["id_observacion"];
                        dic["nombre_actividad"] = reader["nombre"].ToString();
                        dic["temperatura"] = (float)(double)reader["temperatura"];
                        dic["humedad"] = (float)(double)reader["humedad"];
                        dic["fecha_hora"] = (DateTime)reader["fecha_hora"];
                        dic["descripcion"] = reader["descripcion"].ToString();
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static int Crear(int idObservacion, DateTime fecha, float humedad, float temperatura, String descripcion)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "INSERT INTO ronda_de_observacion(id_observacion, fecha_hora, humedad, temperatura, descripcion) " +
                    "OUTPUT Inserted.id_ronda VALUES(@id_observacion, @fecha, @humedad, @temperatura, @descripcion)", conn);

                var idP = new SqlParameter("@id_observacion", SqlDbType.Int, 0) { Value = idObservacion };
                var fechaP = new SqlParameter("@fecha", SqlDbType.DateTime, 0) { Value = fecha };
                var humedadP = new SqlParameter("@humedad", SqlDbType.Float, 0) { Value = humedad };
                var temperaturaP = new SqlParameter("@temperatura", SqlDbType.Float, 0) { Value = temperatura };
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200) { Value = descripcion };

                command.Parameters.Add(idP);
                command.Parameters.Add(fechaP);
                command.Parameters.Add(humedadP);
                command.Parameters.Add(temperaturaP);
                command.Parameters.Add(descripcionP);

                command.Prepare();

                var res = (int)command.ExecuteScalar();

                conn.Close();

                return res;
            }
        }

        public static void Modificar(int idRonda, int idObservacion, DateTime fecha, 
            float humedad, float temperatura, String descripcion)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "UPDATE ronda_de_observacion SET id_observacion = @id_observacion, fecha_hora = @fecha, " +
                    "descripcion = @descripcion, humedad = @humedad, temperatura = @temperatura WHERE id_ronda = @id", conn);

                var idP = new SqlParameter("@id_observacion", SqlDbType.Int, 0) { Value = idObservacion };
                var fechaP = new SqlParameter("@fecha", SqlDbType.DateTime, 0) { Value = fecha };
                var humedadP = new SqlParameter("@humedad", SqlDbType.Float, 0) { Value = humedad };
                var temperaturaP = new SqlParameter("@temperatura", SqlDbType.Float, 0) { Value = temperatura };
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200) { Value = descripcion };
                var idR = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idRonda };

                command.Parameters.Add(idP);
                command.Parameters.Add(idR);
                command.Parameters.Add(fechaP);
                command.Parameters.Add(humedadP);
                command.Parameters.Add(temperaturaP);
                command.Parameters.Add(descripcionP);

                command.Prepare();

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void Eliminar(int idRonda)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                var command = new SqlCommand("DELETE FROM ronda_de_observacion WHERE id_ronda = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idRonda };
                command.Parameters.Add(idP);
                command.Prepare();
                command.ExecuteNonQuery();
            }
        }
    }
}