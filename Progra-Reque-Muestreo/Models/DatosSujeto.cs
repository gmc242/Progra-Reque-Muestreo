using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosSujeto
    {
        public static List<Tuple<int, String>> GetSujetosDeProyecto(int idProyecto)
        {
            var lista = new List<Tuple<int, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT id_sujeto, nombre FROM sujetos_de_prueba WHERE id_proyecto = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0)
                {
                    Value = idProyecto
                };

                command.Parameters.Add(idP);

                command.Prepare();
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Tuple<int, string>((int)reader["id_sujeto"], reader["nombre"].ToString()));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static Dictionary<String, dynamic> GetSujeto(int idSujeto)
        {
            var dic = new Dictionary<String, dynamic>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT nombre, id_proyecto FROM sujetos_de_prueba WHERE id_sujeto = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0)
                {
                    Value = idSujeto
                };

                command.Parameters.Add(idP);
                command.Prepare();

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["nombre"] = reader["nombre"].ToString();
                        dic["id_sujeto"] = idSujeto;
                        dic["id_proyecto"] = (int)reader["id_proyecto"];
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static int Crear(String nombre, int idProyecto)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "INSERT INTO sujetos_de_prueba(nombre, id_proyecto) " +
                    "OUTPUT Inserted.id_sujeto VALUES(@nombre, @id)", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0){ Value = idProyecto };
                var nomP = new SqlParameter("@nombre", SqlDbType.VarChar, 40) { Value = nombre };

                command.Parameters.Add(idP);
                command.Parameters.Add(nomP);
                command.Prepare();

                var res = (int)command.ExecuteScalar();
                
                conn.Close();

                return res;
            }
        }

        public static void Modificar(int idSujeto, String nombre, int idProyecto)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "UPDATE sujetos_de_prueba SET nombre = @nombre, id_proyecto = @id_proyecto " +
                    "WHERE id_sujeto = @id_sujeto", conn);

                var idP = new SqlParameter("@id_proyecto", SqlDbType.Int, 0) { Value = idProyecto };
                var nomP = new SqlParameter("@nombre", SqlDbType.VarChar, 40) { Value = nombre };
                var idS = new SqlParameter("@id_sujeto", SqlDbType.Int, 0) { Value = idSujeto };

                command.Parameters.Add(idP);
                command.Parameters.Add(nomP);
                command.Parameters.Add(idS);
                command.Prepare();

                command.ExecuteNonQuery();

                conn.Close();

            }
        }

        public static void Eliminar(int idSujeto)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM sujetos_de_prueba WHERE id_sujeto = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0) { Value = idSujeto };
                command.Parameters.Add(idP);
                command.Prepare();

                command.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}