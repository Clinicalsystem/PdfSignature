using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Models
{
    public class Document
    {
        public string FileName { get; private set; }
        public Document(string fileName)
        {
            FileName = fileName;
        }
    }
}
