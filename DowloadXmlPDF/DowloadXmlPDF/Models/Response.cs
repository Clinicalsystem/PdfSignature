﻿namespace DowloadXmlPDF.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Object { get; set; }
        public int Status { get; set; }
    }
}