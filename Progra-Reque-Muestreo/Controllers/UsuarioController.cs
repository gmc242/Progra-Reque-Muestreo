using Progra_Reque_Muestreo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Progra_Reque_Muestreo.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario/
        public ActionResult Index()
        {
            List<String> usuarios = DatosUsuarios.getUsuariosString();
            ViewData["usuarios"] = usuarios;

            return View();
        }

        // GET: Usuario/Registrar
        public ActionResult Registrar()
        {
            return View();
        }

        // POST: Usuario/Registrar
        [HttpPost]
        public ActionResult Registrar(FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var pass = collection["pass"].ToString();
                var id = collection["id"].ToString();
                var tipo = bool.Parse(collection["tipo"].ToString());

                DatosUsuarios.agregarUsuario(usuario: id, nombre: nombre, pass: pass, tipo: tipo);

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Usuario/Editar/5
        public ActionResult Editar(String id)
        {
            try
            {
                var nombre = DatosUsuarios.getNombreUsuario(id);
                if (nombre != null && !String.IsNullOrEmpty(nombre))
                {
                    ViewData["nombre"] = nombre;
                    ViewData["id"] = id;
                    ViewData["tipo"] = DatosUsuarios.IsAdmin(id);
                    return View();
                }
                else
                    throw new Exception("No se ha encontrado el usuario en la base de datos");
            }catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        public ActionResult Editar(String id, FormCollection collection)
        {
            try
            {
                var nombre = collection["nombre"].ToString();
                var pass = collection["pass"].ToString();
                var idNew = collection["id"].ToString();
                var idOld = collection["idOld"].ToString();
                var tipo = bool.Parse(collection["tipo"]);

                if (String.IsNullOrEmpty(nombre) || String.IsNullOrEmpty(id))
                    throw new Exception("Los campos de id y nombre no pueden estar vacíos.");

                DatosUsuarios.editar(idOld, idNew, nombre, pass, tipo);

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Usuario/Borrar/5
        public ActionResult Borrar(String id)
        {
            try
            {
                if (id != "admin")
                    if (DatosUsuarios.eliminar(id))
                        return RedirectToAction("Index");
                    else
                        throw new Exception("No se ha podido borrar el usuario de la base de datos");
                else
                    throw new Exception("El usuario administrador no se puede borrar");
            }
            catch (Exception e)
            {
                ViewData["exception"] = e;
                return View("Error");
            }
        }

        // GET: Usuario/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            var usuario = form["id"].ToString();
            var pass = form["pass"].ToString();

            try
            {
                if (DatosUsuarios.iniciarSesion(usuario: usuario, pass: pass))
                {
                    Session["usuario"] = usuario;
                    return RedirectToAction("Index", "Home");
                }
                else
                    throw new Exception("Los datos de inicio de sesión no son válidos");
            }catch(Exception e)
            {
                ViewData["Exception"] = e;
                return View("Error");
            }
        }
    }
}
