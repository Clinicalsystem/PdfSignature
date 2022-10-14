using System;
using System.Collections.Generic;
using System.Text;

namespace PdfSignature.Services
{
    public interface IToast
    {
        void Show(string message);
    }
}
