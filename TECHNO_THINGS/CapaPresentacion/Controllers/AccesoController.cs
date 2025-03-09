using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CapaPresentacion.Controllers
{
    public class AccesoController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario objeto)
        {
            int resultado;
            string mensaje = string.Empty;

            //utilizamos ViewData para almacenar temporalmente la información que el usuario coloca en el formulario

            ViewData["Nombre"] = string.IsNullOrEmpty(objeto.Nombre) ? "" : objeto.Nombre;
            ViewData["Apellido"] = string.IsNullOrEmpty(objeto.Apellido) ? "" : objeto.Apellido;
            ViewData["Correo"] = string.IsNullOrEmpty(objeto.Correo) ? "" : objeto.Correo;

            if (objeto.Clave != objeto.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = new CN_Usuario().Registrar(objeto, out mensaje, false);

            if (resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }


        [HttpPost]
        public ActionResult Login(string correo, string clave)
        {
            Usuario oUsuario = new Usuario();
            oUsuario = new CN_Usuario().Listar().Where(u => u.Correo == correo && u.Clave == CN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña incorrecta";
                //Error es el nombre del ViewBag, podemos poner cualquiera
                return View();
            }
            else
            {
                //compruebo si el usuario accede por primera vez
                if (oUsuario.Restablecer)
                {
                    TempData["IdUsuario"] = oUsuario.Id;

                    return RedirectToAction("CambiarClave"); //no es necesario aclarar el controlador porque esta dentro del mismo
                }
                else
                {
                    //creando la autenticación del usuario por su correo
                    FormsAuthentication.SetAuthCookie(oUsuario.Correo, false);

                    Session["Usuario"] = oUsuario;

                    ViewBag.Error = null;

                    if (oUsuario.IsAdmin)
                    {
                        return RedirectToAction("Index", "Editor");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Tienda");
                    }
                }

            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idusuario, string claveactual, string nuevaclave, string confirmaclave)
        {
            Usuario oUsuario = new Usuario();

            oUsuario = new CN_Usuario().Listar().Where(u => u.Id == int.Parse(idusuario)).FirstOrDefault();

            if (oUsuario.Clave != CN_Recursos.ConvertirSha256(claveactual))
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if (nuevaclave != confirmaclave)
            {
                TempData["IdUsuario"] = idusuario;
                ViewData["vclave"] = claveactual;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";
            nuevaclave = CN_Recursos.ConvertirSha256(nuevaclave);

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuario().CambiarClave(int.Parse(idusuario), nuevaclave, out mensaje);

            if (respuesta)
            {
                return RedirectToAction("Login");
            }
            else
            {
                TempData["IdUsuario"] = idusuario;  //para no perder la informacion
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Restablecer(string correo)
        {
            Usuario ousuario = new Usuario();
            ousuario = new CN_Usuario().Listar().Where(item => item.Correo == correo).FirstOrDefault();

            if (ousuario == null)
            {
                ViewBag.Error = "No se encontró un usuario relacionado a ese correo";
                return View();
            }

            string mensaje = string.Empty;
            bool respuesta = new CN_Usuario().RestablecerClave(ousuario.Id, correo, out mensaje);

            if (respuesta)
            {
                ViewBag.Error = null;
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Tienda");

        }


    }


}