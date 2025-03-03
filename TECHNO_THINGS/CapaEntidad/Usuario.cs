using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Correo { get; set; }

        public string Clave { get; set; }

        public string ConfirmarClave { get; set; }

        public bool Reestablecer { get; set; }

        public bool Activo { get; set; }

        public string FechaRegistro { get; set; }

        public bool IsAdmin { get; set; }
    }
}
