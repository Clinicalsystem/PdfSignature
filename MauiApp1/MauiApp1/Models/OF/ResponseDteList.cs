using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1.Models.OF
{
    public class ResponseDteList
    {
        public int current_page { get; set; }

        public int last_page { get; set; }

        public List<Data> Data { get; set; }

        public int total { get; set; }
    }
}
