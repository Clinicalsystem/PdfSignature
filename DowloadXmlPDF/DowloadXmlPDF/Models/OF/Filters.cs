using System.Text.Json.Serialization;

namespace DowloadXmlPDF.Models.OF
{
    public class Filters
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Page { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public RUTRecep RUTRecep { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public FchEmis FchEmis { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public TipoDTE TipoDTE { get; set; }
    }

    public class RUTRecep
    {
        /// <summary>
        /// MenorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lt { get; set; }

        /// <summary>
        /// MenorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lte { get; set; }

        /// <summary>
        /// EsIgual
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string eq { get; set; }

        /// <summary>
        /// MayorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gte { get; set; }

        /// <summary>
        /// MayorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gt { get; set; }

        /// <summary>
        /// EsDiferenteQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ne { get; set; }

    }
    public class FchEmis
    {
        /// <summary>
        /// MenorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lt { get; set; }

        /// <summary>
        /// MenorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lte { get; set; }

        /// <summary>
        /// EsIgual
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string eq { get; set; }

        /// <summary>
        /// MayorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gte { get; set; }

        /// <summary>
        /// MayorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gt { get; set; }

        /// <summary>
        /// EsDiferenteQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ne { get; set; }

        public void DateIsMenorQue(DateTime date)
        {
            lt = date.ToString("yyyy-MM-dd");
        }
        public void DateIsMayorQue(DateTime date)
        {
            gt = date.ToString("yyyy-MM-dd");
        }
        public void DateIsIgualQue(DateTime date)
        {
            eq = date.ToString("yyyy-MM-dd");
        }
        public void DateIsRangue(DateTime dateIni, DateTime dateFin)
        {
            gte = dateIni.ToString("yyyy-MM-dd");
            lte = dateFin.ToString("yyyy-MM-dd");
        }
    }
    public class TipoDTE
    {
        /// <summary>
        /// MenorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lt { get; set; }

        /// <summary>
        /// MenorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string lte { get; set; }

        /// <summary>
        /// EsIgual
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string eq { get; set; }

        /// <summary>
        /// MayorOigualQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gte { get; set; }

        /// <summary>
        /// MayorQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string gt { get; set; }

        /// <summary>
        /// EsDiferenteQue
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string ne { get; set; }
    }


}
