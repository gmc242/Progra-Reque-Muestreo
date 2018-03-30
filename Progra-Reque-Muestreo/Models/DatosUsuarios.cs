using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Progra_Reque_Muestreo.Models
{
    public class DatosUsuarios
    {
        
        public static Boolean revisarCredenciales(String usuario)
        {
            var ctx = HttpContext.Current;
            if (ctx.Session["usuario"] != null)
                return ctx.Session["usuario"].ToString() == usuario;
            else
                return false;
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
                            return true;
                        }
                        else
                            return false;
                        
                    }
                }

                conn.Close();
            }

            return false;
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

        public static void agregarUsuario(String usuario, String nombre, String pass)
        {
            if(revisarCredenciales("admin"))
            {
                using (var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    var stmn = new SqlCommand("INSERT INTO usuario(identificador, nombre, sal, pass_hash) " +
                        "VALUES(@id, @nom, @sal, @pass)", conn);

                    var id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                    var nombreP = new SqlParameter("@nom", System.Data.SqlDbType.VarChar, 40);
                    var sal = new SqlParameter("@sal", System.Data.SqlDbType.Binary, 32);
                    var passP = new SqlParameter("@pass", System.Data.SqlDbType.Binary, 64);

                    id.Value = usuario;
                    nombreP.Value = nombre;

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

        public static void editar(String idActual, String usuario, String nombre, String pass)
        {
            if (revisarCredenciales("admin") || revisarCredenciales(usuario))
            {
                using (var conn = ControladorGlobal.GetConn())
                {
                    conn.Open();

                    SqlCommand stmn;

                    if (!String.IsNullOrEmpty(pass))
                        stmn = new SqlCommand("UPDATE usuario " +
                            "SET identificador = @id," +
                            "nombre = @nom, " +
                            "sal = @sal, " +
                            "pass_hash = @pass " +
                            "WHERE identificador = @idActual", conn);
                    else
                        stmn = new SqlCommand("UPDATE usuario " +
                            "SET identificador = @id," +
                            "nombre = @nom " +
                            "WHERE identificador = @idActual", conn);

                    var id = new SqlParameter("@id", System.Data.SqlDbType.VarChar, 20);
                    var nombreP = new SqlParameter("@nom", System.Data.SqlDbType.VarChar, 40);
                    var idActualP = new SqlParameter("@idActual", System.Data.SqlDbType.VarChar, 20);

                    id.Value = usuario;
                    nombreP.Value = nombre;
                    idActualP.Value = idActual;

                    stmn.Parameters.Add(id);
                    stmn.Parameters.Add(nombreP);
                    stmn.Parameters.Add(idActualP);

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
            if(revisarCredenciales("admin") || revisarCredenciales(id))
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