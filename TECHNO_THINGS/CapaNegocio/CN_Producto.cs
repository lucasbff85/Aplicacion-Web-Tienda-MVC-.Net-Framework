using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private CD_Producto objCapaDatos = new CD_Producto();

        public List<Producto> Listar()
        {
            return objCapaDatos.Listar();
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede estar vacío";

            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del producto no puede estar vacía";

            }

            else if (obj.oMarca.Id == 0)
            {
                Mensaje = "Debes seleccionar una marca";

            }

            else if (obj.oCategoria.Id == 0)
            {
                Mensaje = "Debes seleccionar una categoría";

            }

            else if (obj.Precio <= 0)
            {
                Mensaje = "Debe colocar un precio correcto";
            }

            else if (obj.Stock <= 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }

            //si no hay errores, registra
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.Registrar(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El nombre del Producto no puede estar vacío";

            }

            else if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "La descripción del producto no puede estar vacía";

            }

            else if (obj.oMarca.Id == 0)
            {
                Mensaje = "Debes seleccionar una marca";

            }

            else if (obj.oCategoria.Id == 0)
            {
                Mensaje = "Debes seleccionar una categoría";

            }

            else if (obj.Precio <= 0)
            {
                Mensaje = "Debe colocar un precio correcto";
            }

            else if (obj.Stock <= 0)
            {
                Mensaje = "Debe ingresar el stock del producto";
            }

            //si no hay errores, edita
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDatos.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool GuardarDatosImagen(Producto obj, out string Mensaje)
        {
            return objCapaDatos.GuardarDatosImagen(obj, out Mensaje);
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDatos.Eliminar(id, out Mensaje);
        }


    }
}
