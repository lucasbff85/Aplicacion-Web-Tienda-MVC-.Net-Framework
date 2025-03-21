using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Ciudad
    {
        public int Id { get; set; }

        public string Descripcion { get; set; }

        public Provincia oProvincia { get; set; }

        public string CP { get; set; }
    }
}
