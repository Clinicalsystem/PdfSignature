using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PdfSignature.Implementation
{
    public interface IFileManager
    {
        Task<string>  Save(MemoryStream fileStream, string fileName);

    }
}
