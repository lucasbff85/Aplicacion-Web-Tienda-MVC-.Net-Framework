﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Carrito
    {
        public int Id { get; set; }

        public Usuario oCliente { get; set; }

        public Producto oProducto { get; set; }

        public int Cantidad { get; set; }

    }
}