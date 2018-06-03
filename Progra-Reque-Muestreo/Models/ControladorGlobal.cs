using Progra_Reque_Muestreo.Controllers;
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
            ConnectionStringSettings connSettings = ConfigurationManager.ConnectionStrings["SQLConn2"];
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

        public static String GetDateFormat()
        {
            return "yyyy-MM-dd";
        }

        public static void InicializarUsuarios()
        {
            DatosUsuarios.agregarUsuario("admin", "Usuario Administrador", "admin", true);
        }

        public static bool ObtenerStatusCantidad(int idOp, int idProy)
        {
            try
            {
                var proyecto = DatosProyecto.GetProyecto(idProy);
                var cantidadActual = DatosObservacionTarea.GetCantidadObservacionesPorOperacion(idOp);
                return cantidadActual >= proyecto["tamano_muestreo"];
            }catch(Exception e)
            {
                throw e;
            }
        }

        public static String ObtenerStatusCantidadString(int idOp, int idProy)
        {
            try
            {
                if(ObtenerStatusCantidad(idOp, idProy))
                {
                    return "Ha alcanzado el tamaño de muestreo deseado";
                }
                else
                {
                    return "No ha alcanzado la cantidad de observaciones deseadas";
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        
    }
}