namespace DowloadXmlPDF.Models.OF
{
    public class Data
    {
        public int RUTRecep { get; set; }
        public string DV { get; set; }
        public string RznSocRecep { get; set; }
        public int TipoDTE { get; set; }
        public int Folio { get; set; }
        public string FchEmis { get; set; }
        public string FechaRecibido { get; set; }
        public int MntExe { get; set; }
        public int MntNeto { get; set; }
        public int IVA { get; set; }
        public int MntTotal { get; set; }
        public int FmaPago { get; set; }
        public string Token { get; set; }

        // [JsonIgnore]
        public string RutDv => RUTRecep > 0 ? $"{RUTRecep}-{DV}" : String.Empty;
        // [JsonIgnore]
        public bool IsSelected { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}", RutDv, RznSocRecep, TipoDTE, Folio, FchEmis, FechaRecibido, MntExe, MntNeto, IVA, MntTotal, Token);
        }
    }
}
