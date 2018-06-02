using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public static class DatosUsuarios
    {
        public static String obtenerUsuarioActivo()
        {
            var ctx = HttpContext.Current;
            if(ctx.Session["usuario"] != null)
            {
                return ctx.Session["usuario"].ToString();
            }
            else
            {
                throw new Exception("No se ha iniciado sesión con ningún usuario");
            }
        }

        public static Boolean IsAdmin(String usuario)
        {
            bool admin = false;

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                SqlCommand command = new SqlCommand(
                    "SELECT administrador FROM usuario WHERE identificador = @usuario", conn);

                SqlParameter usuarioP = new SqlParameter("@usuario", System.Data.SqlDbType.VarChar, 20)
                {
                    Value = usuario
                };

                command.Parameters.Add(usuarioP);
                command.Prepare();

                var temp = command.ExecuteScalar();
                admin = bool.Parse(temp.ToString());

                conn.Close();
            }

            return admin;
        }

        public static Boolean VerificarCredencialesAdmin()
        {
            try
            {
                var usuario = obtenerUsuarioActivo();
                bool admin = false;

                return IsAdmin(usuario);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static Boolean VerificarCredencialesLider(int idProy)
        {
            var usuario = obtenerUsuarioActivo();
            var lider = false;

            using(var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var cmd = new SqlCommand(
                    "SELECT lider_id FROM proyecto WHERE id_proyecto = @id", conn);

                var par = new SqlParameter("@id", System.Data.SqlDbType.Int, 0)
                {
                    Value = idProy
                };

                cmd.Parameters.Add(par);
                cmd.Prepare();

                var liderObt = cmd.ExecuteScalar().ToString();
                lider = (liderObt.Equals(usuario));

                conn.Close();
            }

            return lider;
        }

        public static Boolean VerificarCredencialesOperacion(int idOperacion)
        {
            try
            {
                bool credenciales = false;
                var usuario = obtenerUsuarioActivo();

                using(var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    var command = new SqlCommand(
                        "SELECT ua.id_actividad, a.nombre " +
                        "FROM usuarios_por_actividad AS ua " +
                        "INNER JOIN actividad AS a" +
                        "ON ua.id_actividad = a.id_actividad" +
                        "WHERE ua.id_usuario = @usuario " +
                        "AND ua.id_actividad = @operacion", conn);

                    var parUsuario = new SqlParameter("@usuario", System.Data.SqlDbType.VarChar, 20)
                    {
                        Value = usuario
                    };

                    var parActividad = new SqlParameter("@operacion", System.Data.SqlDbType.Int, 0)
                    {
                        Value = idOperacion
                    };

                    command.Parameters.Add(parUsuario);
                    command.Parameters.Add(parActividad);

                    command.Prepare();

                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            credenciales = true;
                            break;
                        }
                    }

                    conn.Close();
                }

                return credenciales;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static Boolean iniciarSesion(String usuario, String pass)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT sal, pass_hash " +
                    "FROM usuario WHERE identificador = @id", conn);

                var id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                id.Value = usuario;
                command.Parameters.Add(id);
                command.Prepare();

                Boolean res = false;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var salTemp = (Byte[])reader["sal"];

                        byte[] passDB = (Byte[])reader["pass_hash"];
                        byte[] passIngresado = (Encoding.UTF8.GetBytes(pass));

                        byte[] saltedIngresado = new Byte[passIngresado.Length + salTemp.Length];

                        passIngresado.CopyTo(saltedIngresado, 0);
                        salTemp.CopyTo(saltedIngresado, passIngresado.Length);

                        byte[] passTempIngresado = new SHA256Managed().ComputeHash(saltedIngresado);

                        if (passDB.SequenceEqual(passTempIngresado))
                        {
                            HttpContext.Current.Session["usuario"] = id;
                            res = true;
                        }
                        else
                            res = false;
                        
                    }
                }

                conn.Close();

                return res;
            }
        }

        public static String getNombreUsuario(String id)
        {
            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand(
                    "SELECT nombre FROM usuario WHERE identificador = @id", conn);

                var idP = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                idP.Value = id;

                command.Parameters.Add(idP);
                command.Prepare();
                
                var usuario = command.ExecuteScalar().ToString();
                conn.Close();

                return usuario;
            }
        }

        public static List<String> getUsuariosString()
        {
            var res = new List<String>();

            using (var conn = ControladorGlobal.GetConn())
            {
                conn.Open();

                var command = new SqlCommand("SELECT nombre, identificador " +
                    "FROM usuario", conn);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        String s = reader["nombre"].ToString() + " Id: " + reader["identificador"].ToString();
                        res.Add(s);
                    }
                }

                conn.Close();
            }

            return res;
        }

        public static void agregarUsuario(String usuario, String nombre, String pass, bool tipo)
        {
            if(VerificarCredencialesAdmin())
            {
                using (var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    var stmn = new SqlCommand("INSERT INTO usuario(identificador, nombre, administrador, sal, pass_hash) " +
                        "VALUES(@id, @nom, @tipo, @sal, @pass)", conn);

                    var id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                    var nombreP = new SqlParameter("@nom", System.Data.SqlDbType.VarChar, 40);
                    var sal = new SqlParameter("@sal", System.Data.SqlDbType.Binary, 32);
                    var passP = new SqlParameter("@pass", System.Data.SqlDbType.Binary, 64);
                    var tipoP = new SqlParameter("@tipo", System.Data.SqlDbType.Bit, 0);

                    id.Value = usuario;
                    nombreP.Value = nombre;
                    tipoP.Value = tipo;

                    // Genera una sal de 32 bits y genera un hash para el pass junto con la sal
                    using (var random = new RNGCryptoServiceProvider())
                    {
                        var salTemp = new byte[32];
                        random.GetNonZeroBytes(salTemp);

                        byte[] passB = (Encoding.UTF8.GetBytes(pass));
                        byte[] salted = new Byte[passB.Length + salTemp.Length];
                        passB.CopyTo(salted, 0);
                        salTemp.CopyTo(salted, passB.Length);

                        byte[] passTemp = new SHA256Managed().ComputeHash(salted);

                        sal.Value = salTemp;
                        passP.Value = passTemp;
                    }

                    stmn.Parameters.Add(id);
                    stmn.Parameters.Add(nombreP);
                    stmn.Parameters.Add(sal);
                    stmn.Parameters.Add(passP);
                    stmn.Parameters.Add(tipoP);

                    stmn.Prepare();
                    stmn.ExecuteNonQuery();

                    conn.Close();
                }
            }
            else
            {
                throw new Exception("No tiene los privilegios necesarios");
            }
        }

        public static void editar(String idActual, String usuario, String nombre, String pass, bool tipo)
        {
            if (VerificarCredencialesAdmin() || obtenerUsuarioActivo().Equals(usuario))
            {
                using (var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    SqlCommand stmn;

                    if (!String.IsNullOrEmpty(pass))
                        stmn = new SqlCommand("UPDATE usuario " +
                            "SET identificador = @id, " +
                            "nombre = @nom, " +
                            "sal = @sal, " +
                            "pass_hash = @pass " +
                            "tipo = @tipo " +
                            "WHERE identificador = @idActual", conn);
                    else
                        stmn = new SqlCommand("UPDATE usuario " +
                            "SET identificador = @id, " +
                            "nombre = @nom " +
                            "tipo = @tipo " +
                            "WHERE identificador = @idActual", conn);

                    var id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                    var nombreP = new SqlParameter("@nom", System.Data.SqlDbType.VarChar, 40);
                    var idActualP = new SqlParameter("@idActual", System.Data.SqlDbType.VarChar, 20);
                    var tipoP = new SqlParameter("@tipo", System.Data.SqlDbType.Bit, 0);

                    id.Value = usuario;
                    nombreP.Value = nombre;
                    idActualP.Value = idActual;

                    stmn.Parameters.Add(id);
                    stmn.Parameters.Add(nombreP);
                    stmn.Parameters.Add(idActualP);
                    stmn.Parameters.Add(tipoP);

                    if (!String.IsNullOrEmpty(pass))
                    {
                        var sal = new SqlParameter("@sal", System.Data.SqlDbType.Binary, 32);
                        var passP = new SqlParameter("@pass", System.Data.SqlDbType.Binary, 64);

                        // Genera una sal de 32 bits y genera un hash para el pass junto con la sal
                        using (var random = new RNGCryptoServiceProvider())
                        {
                            var salTemp = new byte[32];
                            random.GetNonZeroBytes(salTemp);

                            byte[] passB = (Encoding.UTF8.GetBytes(pass));
                            byte[] salted = new Byte[passB.Length + salTemp.Length];
                            passB.CopyTo(salted, 0);
                            salTemp.CopyTo(salted, passB.Length);

                            byte[] passTemp = new SHA256Managed().ComputeHash(salted);

                            sal.Value = salTemp;
                            passP.Value = passTemp;
                        }

                        stmn.Parameters.Add(sal);
                        stmn.Parameters.Add(passP);
                    }

                    stmn.Prepare();
                    stmn.ExecuteNonQuery();

                    conn.Close();
                }
            }
            else
            {
                throw new Exception("No tiene los privilegios necesarios");
            }
        }

        public static Boolean eliminar(String id)
        {
            if(VerificarCredencialesAdmin() || obtenerUsuarioActivo().Equals(id))
            {
                using (var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    var command = new SqlCommand(
                        "DELETE FROM usuario WHERE identificador = @id", conn);

                    var idP = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                    idP.Value = id;
                    command.Parameters.Add(idP);
                    command.Prepare();

                    var res = command.ExecuteNonQuery();

                    conn.Close();

                    return res > 0;
                }
            }
            else
            {
                throw new Exception("No tiene los privilegios necesarios");
            }
        }

    }
}