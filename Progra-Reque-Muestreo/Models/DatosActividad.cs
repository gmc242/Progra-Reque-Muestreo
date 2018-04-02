using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosActividad
    {
        public static int CrearActividad(int idProyecto, String nombre, String descripcion, String[] usuarios)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "INSERT INTO actividad(nombre, descripcion, id_proyecto) OUTPUT Inserted.id_actividad " +
                    "VALUES(@nombre, @descripcion, @id_proyecto)", conn);

                var nombreP = new SqlParameter("@nombre", SqlDbType.VarChar, 20);
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200);
                var idP = new SqlParameter("@id_proyecto", SqlDbType.Int, 0);

                nombreP.Value = nombre;
                descripcionP.Value = descripcion;
                idP.Value = idProyecto;

                command.Parameters.Add(nombreP);
                command.Parameters.Add(descripcionP);
                command.Parameters.Add(idP);

                command.Prepare();
                var res = (int)command.ExecuteScalar();

                if (usuarios != null)
                    AgregarUsuariosActividad(res, usuarios);

                conn.Close();

                return res;
            }
        }

        public static void ModificarActividad(int idActividad, int idProyecto, String nombre, String descripcion, String[] usuarios)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "UPDATE actividad SET nombre = @nombre, descripcion = @descripcion, " +
                    "id_proyecto = @id_proyecto WHERE id_actividad = @id", conn);

                var nombreP = new SqlParameter("@nombre", SqlDbType.VarChar, 20);
                var descripcionP = new SqlParameter("@descripcion", SqlDbType.VarChar, 200);
                var idP = new SqlParameter("@id_proyecto", SqlDbType.Int, 0);
                var idA = new SqlParameter("@id", SqlDbType.Int, 0);

                nombreP.Value = nombre;
                descripcionP.Value = descripcion;
                idP.Value = idProyecto;
                idA.Value = idActividad;

                command.Parameters.Add(nombreP);
                command.Parameters.Add(descripcionP);
                command.Parameters.Add(idP);
                command.Parameters.Add(idA);

                command.Prepare();
                command.ExecuteNonQuery();

                if (usuarios != null)
                    ModificarUsuariosActividad(idActividad, usuarios);

                conn.Close();
            }
        }

        public static Dictionary<String, dynamic> getActividad(int idActividad)
        {
            var dic = new Dictionary<String, dynamic>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM actividad WHERE id_actividad = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idActividad;
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dic["nombre"] = reader["nombre"].ToString();
                        dic["descripcion"] = reader["descripcion"].ToString();
                        dic["id_actividad"] = (int)reader["id_actividad"];
                        dic["id_proyecto"] = (int)reader["id_proyecto"];
                        dic["usuarios"] = getUsuariosPorActividad(dic["id_actividad"]);
                    }
                }

                conn.Close();
            }

            return dic;
        }

        public static List<Tuple<int, String>> getActividades(int idProyecto)
        {
            var lista = new List<Tuple<int, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT id_actividad, nombre FROM actividad WHERE id_proyecto = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idProyecto;
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Tuple<int, string>((int)reader["id_actividad"], reader["nombre"].ToString()));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static List<Tuple<String, String>> getUsuariosParaProyecto(int idProyecto)
        {
            var lista = new List<Tuple<String, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT identificador, nombre " +
                    "FROM usuario AS u INNER JOIN asistentes_por_proyecto AS ap " +
                    "ON u.identificador = ap.id_asistente " +
                    "WHERE ap.id_proyecto = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idProyecto;
                command.Parameters.Add(idP);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Tuple<String, String>(reader["identificador"].ToString(),
                            reader["nombre"].ToString()));
                    }
                }

                conn.Close();
            }

            return lista;
        }

        public static List<Tuple<String, String>> getUsuariosPorActividad(int idActividad)
        {
            var lista = new List<Tuple<String, String>>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT identificador, nombre FROM usuario AS u " +
                    "INNER JOIN usuarios_por_actividad AS up ON u.identificador = up.id_usuario " +
                    "WHERE up.id_actividad = @id", conn);

                var idA = new SqlParameter("@id", SqlDbType.Int, 0);
                idA.Value = idActividad;
                command.Parameters.Add(idA);
                command.Prepare();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(new Tuple<string, string>(
                            reader["identificador"].ToString(), reader["nombre"].ToString()));
                }

                conn.Close();
            }

            return lista;
        }

        public static void AgregarUsuariosActividad(int idActividad, String[] usuarios)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                foreach(String idUsuario in usuarios)
                {
                    var command = new SqlCommand(
                    "INSERT INTO usuarios_por_actividad(id_actividad, id_usuario) " +
                    "VALUES(@id_actividad, @id_usuario)", conn);

                    var idA = new SqlParameter("@id_actividad", SqlDbType.Int, 0);
                    var idU = new SqlParameter("@id_usuario", SqlDbType.VarChar, 20);

                    idA.Value = idActividad;
                    idU.Value = idUsuario;

                    command.Parameters.Add(idA);
                    command.Parameters.Add(idU);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        public static void ModificarUsuariosActividad(int idActividad, String[] usuarios)
        {
            EliminarUsuariosActividad(idActividad);

            AgregarUsuariosActividad(idActividad, usuarios);
        }

        public static void EliminarDeProyecto(int idProyecto)
        {
            var actividades = getActividades(idProyecto);
            foreach(Tuple<int,String> actividad in actividades)
            {
                EliminarActividad(actividad.Item1);
            }
        }

        public static void EliminarActividad(int idActividad)
        {
            //Primero elimina las tuplas relacionadas con la actividad

            //DatosTarea.EliminarDeActividad(idActividad);
            //DatosObservacion.EliminarDeActividad(idActividad);
            //DatosRonda.EliminarDeActividad(idActividad);
            EliminarUsuariosActividad(idActividad);

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "DELETE FROM actividad WHERE id_actividad = @id", conn);

                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = idActividad;

                command.Parameters.Add(idP);
                command.Prepare();

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        public static void EliminarUsuariosActividad(int idActividad)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "DELETE FROM usuarios_por_actividad WHERE id_actividad = @id", conn);

                var id = new SqlParameter("@id", SqlDbType.Int, 0);
                id.Value = idActividad;

                command.Parameters.Add(id);
                command.Prepare();
                command.ExecuteNonQuery();

                conn.Close();
            }
        }

    }
}