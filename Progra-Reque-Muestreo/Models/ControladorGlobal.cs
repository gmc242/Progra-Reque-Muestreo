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
            DatosUsuarios.agregarUsuario("admin", "Usuario Administrador", "admin");
        }

        

        
    }
}