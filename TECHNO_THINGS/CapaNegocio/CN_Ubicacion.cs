using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Ubicacion
    {
        private CD_Ubicacion objCapaDato = new CD_Ubicacion();

        public List<Provincia> ListarProvincias()
        {
            return objCapaDato.ListarProvincias();
        }


        public List<Ciudad> ListarCiudades()
        {
            return objCapaDato.ListarCiudades();
        }

        public List<Ciudad> ObtenerCiudad( int idProvincia)
        {
            return objCapaDato.ObtenerCiudad( idProvincia);
        }


        public int RegistrarProvincia(Provincia obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la provincia no puede estar vacío";

            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarProvincia(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool EditarProvincia(Provincia obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la provincia no puede estar vacío";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.EditarProvincia(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarProvincia(int id, out string Mensaje)
        {
            return objCapaDato.EliminarProvincia(id, out Mensaje);
        }




        public int RegistrarCiudad(Ciudad obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la ciudad no puede estar vacío";

            }

            if (obj.oProvincia.Id <=0)
            {
                Mensaje = "Debe selaccionar la provincia a la que pertenece";

            }

            if (string.IsNullOrEmpty(obj.CP) || string.IsNullOrWhiteSpace(obj.CP))
            {
                Mensaje = "El Código Postal de la ciudad no puede estar vacío";

            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.RegistrarCiudad(obj, out Mensaje);

            }
            else
            {
                return 0;
            }
        }

        public bool EditarCiudad(Ciudad obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.Descripcion) || string.IsNullOrWhiteSpace(obj.Descripcion))
            {
                Mensaje = "El nombre de la ciudad no puede estar vacío";

            }

            if (obj.oProvincia.Id <= 0)
            {
                Mensaje = "Debe selaccionar la provincia a la que pertenece";

            }

            if (string.IsNullOrEmpty(obj.CP) || string.IsNullOrWhiteSpace(obj.CP))
            {
                Mensaje = "El Código Postal de la ciudad no puede estar vacío";

            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCapaDato.EditarCiudad(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool EliminarCiudad(int id, out string Mensaje)
        {
            return objCapaDato.EliminarCiudad(id, out Mensaje);
        }


    }
}