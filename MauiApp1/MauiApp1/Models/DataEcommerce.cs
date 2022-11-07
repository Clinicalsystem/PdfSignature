﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models
{
    public class DataEcommerce
    {
       
        public string rut { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public string comuna { get; set; }

        public List<Actividades> actividades { get; set; }

        public List<Sucursales> sucursales { get; set; }
    }
    public class Actividades
    {
        public int codigoActividadEconomica { get; set; }
        public string giro { get; set; }
        public string actividadEconomica { get; set; }
        public bool actividadPrincipal { get; set; }
    }
    public class Sucursales
    {
        public int cdgSIISucur { get; set; }
        public string comuna { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string telefono { get; set; }


    }
}
