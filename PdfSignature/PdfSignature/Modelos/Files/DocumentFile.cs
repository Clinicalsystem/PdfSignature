using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfSignature.Modelos.Files
{
    public class DocumentFile : Document
    {
        [JsonIgnore]
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [JsonIgnore]
        public string Path { get; set; }
       
    }
}
