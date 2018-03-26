using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace Progra_Reque_Muestreo.Models
{
    public static class ControladorGlobal
    {
        private static readonly object lockobj = new Object();
        private static Dictionary<String, Object> dict = new Dictionary<string, object>();
        
        public static T getDatoT<T>(String etiqueta)
        {
            lock (lockobj)
            {
                return (T) dict[etiqueta];
            }
        }

        public static void addDato(String etiqueta, Object value)
        {
            dict[etiqueta] = value;
        }

        public static SqlConnection GetConn()
        {
            //Codigo de conexion con SQL
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["SQLConn"];
            if (connSettings == null || String.IsNullOrEmpty(connSettings.ConnectionString))
                throw new Exception("La conexión con la base de datos no se ha podido realizar, debido a que el string de conexión no es válido");

            try
            {
                //Crea una conexión y la añade a una colección de objetos Thread Safe
                return new SqlConnection(connSettings.ConnectionString);
            }
            catch (Exception e)
            {
                //Manejar error de conexion
                return null;
            }
        }

        public static void InicializarUsuarios()
        {
            agregarUsuario("admin", "Usuario Administrador", "admin");
        }

        public static void agregarUsuario(String usuario, String nombre, String pass)
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
    }
}