using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Modelos
{
    public class response
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public object Object { get; set; }
        public int Status { get; set; }
    }
}
