using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosProyecto
    {
        public static void Crear(String nombre, int tiempoObs, DateTime inicio, DateTime final,
        int tamano, int tiempoEntreObservaciones, String idLider, String descripcion, String[] asistentes)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var stmn = new SqlCommand(
                    "INSERT INTO proyecto(nombre, tiempo_muestreo, tamano_muestreo, tiempo_entre_muestreos, " +
                    "fecha_inicio, fecha_fin, lider_id, descripcion) OUTPUT Inserted.id_proyecto " +
                    "VALUES (@nom, @tiempo_muestreo, " +
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
                var idProy = (int)stmn.ExecuteScalar();

                if (asistentes != null)
                    agregarAsistentesAProyecto(idProy, asistentes);

                conn.Close();

            }
        }

        public static void Modificar(int id, String nombre, int tiempoObs, DateTime inicio, DateTime final,
        int tamano, int tiempoEntreObservaciones, String idLider, String descripcion, String[] asistentes)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var stmn = new SqlCommand(
                    "UPDATE proyecto SET nombre = @nom, tiempo_muestreo = @tiempo_muestreo, tamano_muestreo = @tamano_muestreo, " +
                    "tiempo_entre_muestreos = @tiempo_entre_muestreos, fecha_inicio = @fecha_inicio, fecha_fin = @fecha_fin, " +
                    "lider_id = @lider_id, descripcion = @descripcion WHERE id_proyecto = @id", conn);

                var nom = new SqlParameter("@nom", SqlDbType.VarChar, 50);
                var tiempo_muestreo = new SqlParameter("@tiempo_muestreo", SqlDbType.Int, 0);
                var tamano_muestreo = new SqlParameter("@tamano_muestreo", SqlDbType.Int, 0);
                var tiempo_entre_muestreos = new SqlParameter("@tiempo_entre_muestreos", SqlDbType.Int, 0);
                var fecha_inicio = new SqlParameter("@fecha_inicio", SqlDbType.Date, 0);
                var fecha_final = new SqlParameter("@fecha_fin", SqlDbType.Date, 0);
                var lider = new SqlParameter("@lider_id", SqlDbType.VarChar, 40);
                var desc = new SqlParameter("@descripcion", SqlDbType.VarChar, 1000);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0);

                nom.Value = nombre;
                tiempo_muestreo.Value = tiempoObs;
                tiempo_entre_muestreos.Value = tiempoEntreObservaciones;
                tamano_muestreo.Value = tamano;
                fecha_final.Value = final;
                fecha_inicio.Value = inicio;
                desc.Value = descripcion;
                lider.Value = idLider;
                idP.Value = id;

                stmn.Parameters.Add(nom);
                stmn.Parameters.Add(tiempo_muestreo);
                stmn.Parameters.Add(tamano_muestreo);
                stmn.Parameters.Add(tiempo_entre_muestreos);
                stmn.Parameters.Add(fecha_inicio);
                stmn.Parameters.Add(fecha_final);
                stmn.Parameters.Add(lider);
                stmn.Parameters.Add(desc);
                stmn.Parameters.Add(idP);

                stmn.Prepare();
                stmn.ExecuteNonQuery();

                if (asistentes != null)
                    modificarAsistentesEnProyecto(id, asistentes);

                conn.Close();

            }
        }

        public static Dictionary<String, dynamic> GetProyecto(int id)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT * FROM proyecto WHERE id_proyecto = @id",
                    conn);
                var idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = id;
                command.Parameters.Add(idP);
                command.Prepare();

                var dic = new Dictionary<String, dynamic>();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        dic["nombre"] = reader["nombre"].ToString();
                        dic["tiempo_muestreo"] = (int)reader["tiempo_muestreo"];
                        dic["tamano_muestreo"] = (int)reader["tamano_muestreo"];
                        dic["tiempo_entre_muestreos"] = (int)reader["tiempo_entre_muestreos"];
                        dic["fecha_inicio"] = (DateTime)reader["fecha_inicio"];
                        dic["fecha_fin"] = (DateTime)reader["fecha_fin"];
                        dic["lider_id"] = reader["lider_id"].ToString();
                        dic["descripcion"] = reader["descripcion"].ToString();
                        
                    }
                }

                command = new SqlCommand(
                    "SELECT id_asistente FROM asistentes_por_proyecto WHERE id_proyecto = @id", 
                    conn);

                idP = new SqlParameter("@id", SqlDbType.Int, 0);
                idP.Value = id;
                command.Parameters.Add(idP);
                command.Prepare();

                List<String> asistentes = new List<String>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        asistentes.Add(reader["id_asistente"].ToString());
                }

                dic["asistentes"] = asistentes; 

                conn.Close();

                return dic;
            }
        }

        public static List<Tuple<int, String>> GetProyectosString()
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT * FROM proyecto", conn);
                command.Prepare();

                var lista = new List<Tuple<int, String>>();

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var tempTuple = new Tuple<int, String>(
                            (int)reader["id_proyecto"], reader["nombre"].ToString());
                        lista.Add(tempTuple);
                    }
                }

                conn.Close();

                return lista;
            }
        }

        private static void agregarAsistentesAProyecto(int id_proyecto, String[] asistentes)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                foreach (String s in asistentes) {

                    var command = new SqlCommand(
                        "INSERT INTO asistentes_por_proyecto(id_proyecto, id_asistente) " +
                        "VALUES(@idProy, @idAsis)", conn);

                    var idProy = new SqlParameter("@idProy", SqlDbType.Int, 0);
                    var idAsis = new SqlParameter("@idAsis", SqlDbType.VarChar, 20);

                    idProy.Value = id_proyecto;
                    idAsis.Value = s;
                    
                    command.Parameters.Add(idAsis);
                    command.Parameters.Add(idProy);

                    command.Prepare();
                    command.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        private static void modificarAsistentesEnProyecto(int idProy, String[] asistentesNuevos)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "DELETE FROM asistentes_por_proyecto WHERE id_proyecto = @id", conn);
                var id = new SqlParameter("@id", SqlDbType.Int, 0);
                id.Value = idProy;
                command.Parameters.Add(id);
                command.Prepare();
                command.ExecuteNonQuery();

                conn.Close();
            }

            agregarAsistentesAProyecto(idProy, asistentesNuevos);
        }

    }
}