using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objCapaDato = new CD_Usuario();

        public List<Usuario> ListarUsuarios(bool isAdmin)
        {
            return objCapaDato.ListarUsuarios(isAdmin);
        }

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }


        public int Registrar(Usuario obj, out string Mensaje, bool isAdmin)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";

            }
            if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";

            }
            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";

            }


            //si el mensaje sigue vacio es porque no ha ocurrido ningun error
            if (string.IsNullOrEmpty(Mensaje))
            {
                if (isAdmin)
                {
                    string clave = CN_Recursos.GenerarClave();

                    string asunto = "Creación de cuenta";
                    string mensaje_correo = "<h3>Su cuenta fué creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                    mensaje_correo = mensaje_correo.Replace("!clave!", clave);

                    bool respuesta = CN_Recursos.EnviarCorreo(obj.Correo, asunto, mensaje_correo);

                    if (respuesta)
                    {
                        obj.Clave = CN_Recursos.ConvertirSha256(clave);


                        return objCapaDato.RegistrarAdmin(obj, out Mensaje);
                    }
                    else
                    {
                        Mensaje = "No se puede enviar el correo";
                        return 0;
                    }
                }
                else
                {
                    obj.Clave = CN_Recursos.ConvertirSha256(obj.Clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                }
                

            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del usuario no puede estar vacio";

            }
            if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El apellido del usuario no puede estar vacio";

            }
            if (string.IsNullOrEmpty(obj.Correo) || string.IsNullOrWhiteSpace(obj.Correo))
            {
                Mensaje = "El correo del usuario no puede estar vacio";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        public bool CambiarClave(int idusuario, string nuevaclave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idusuario, nuevaclave, out Mensaje);
        }

        public bool RestablecerClave(int idusuario, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaclave = CN_Recursos.GenerarClave();
            bool resultado = objCapaDato.RestablecerClave(idusuario, CN_Recursos.ConvertirSha256(nuevaclave), out Mensaje);

            if (resultado)
            {
                string asunto = "Contraseña reestablecida";
                string mensaje_correo = "<h3>Su cuenta fué reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave!</p>";
                mensaje_correo = mensaje_correo.Replace("!clave!", nuevaclave);

                bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensaje_correo);

                if (respuesta)
                {

                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo";
                    return false;
                }

            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }

        }

    }
}
