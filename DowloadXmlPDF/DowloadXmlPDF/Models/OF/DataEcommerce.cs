namespace DowloadXmlPDF.Models.OF
{
    public class DataEcommerce
    {

        public string rut { get; set; }
        public string razonSocial { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
        public string cdgSIISucur { get; set; }
        public string glosaDescriptiva { get; set; }
        public string direccionRegional { get; set; }
        public string comuna { get; set; }
        public string nombreFantasia { get; set; }

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
