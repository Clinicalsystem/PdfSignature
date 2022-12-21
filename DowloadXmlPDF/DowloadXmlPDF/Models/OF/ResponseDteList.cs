namespace DowloadXmlPDF.Models.OF
{
    public class ResponseDteList
    {
        public int current_page { get; set; }

        public int last_page { get; set; }

        public List<Data> Data { get; set; }

        public int total { get; set; }
    }
}
