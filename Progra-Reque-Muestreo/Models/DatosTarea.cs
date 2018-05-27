using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosTarea
    {
        public static List<Tuple<int, String>> getTareasDeActividad(int idActividad)
        {
            var lista = new List<Tuple<int, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT nombre, id_tarea FROM tarea WHERE id_actividad = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idActividad;
                command.Parameters.Add(idP);
                command.Prepare();
                
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Tuple<int, string>((int)reader["id_tarea"], reader["nombre"].ToString()));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static Dictionary<String, dynamic> getTarea(int idTarea)
        {
            var dic = new Dictionary<String, dynamic>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT nombre, descripcion, id_actividad, categoria FROM tarea WHERE id_tarea = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idTarea;

                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["nombre"] = reader["nombre"].ToString();
                        dic["descripcion"] = reader["descripcion"].ToString();
                        dic["id_actividad"] = (int)reader["id_actividad"];
                        dic["id_tarea"] = idTarea;
                        dic["categoria"] = reader["categoria"].ToString();
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static int CrearTarea(int idActividad, String nombre, String descripcion, String categoria)
        {
            using(var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "INSERT INTO tarea(id_actividad, nombre, descripcion, categoria) " +
                    "OUTPUT Inserted.id_tarea VALUES(@id, @nombre, @descripcion, @categoria)", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                var nombreP = new SqlParameter("@nombre", SqlDbType.VarChar, 20);
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200);
                var cateP = new SqlParameter("@categoria", SqlDbType.VarChar, 2);

                idP.Value = idActividad;
                nombreP.Value = nombre;
                descripcionP.Value = descripcion;
                cateP.Value = categoria;

                command.Parameters.Add(idP);
                command.Parameters.Add(nombreP);
                command.Parameters.Add(descripcionP);
                command.Parameters.Add(cateP);

                int res = (int)command.ExecuteScalar();

                conn.Close();

                return res;
            }
        }

        public static void ModificarTarea(int idTarea, int idActividad, String nombre, String descripcion, String categoria)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("UPDATE tarea SET id_actividad = @id_actividad, " +
                    "nombre = @nombre, descripcion = @descripcion, categoria = @categoria WHERE id_tarea = @id_tarea", conn);

                var idA = new SqlParameter("@id_actividad", SqlDbType.Int, 0);
                var idT = new SqlParameter("@id_tarea", SqlDbType.Int, 0);
                var nombreP = new SqlParameter("@nombre", SqlDbType.VarChar, 20);
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200);
                var cateP = new SqlParameter("@categoria", SqlDbType.VarChar, 2);

                idA.Value = idActividad;
                idT.Value = idTarea;
                nombreP.Value = nombre;
                descripcionP.Value = descripcion;
                cateP.Value = categoria;

                command.Parameters.Add(idA);
                command.Parameters.Add(idT);
                command.Parameters.Add(nombreP);
                command.Parameters.Add(descripcionP);
                command.Parameters.Add(cateP);

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void EliminarTarea(int idTarea)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("DELETE FROM tarea WHERE id_tarea = @id", conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0);

                idP.Value = idTarea;
                command.Parameters.Add(idP);
                command.Prepare();
                command.ExecuteNonQuery();

                conn.Close();
            }
        }
    }
}