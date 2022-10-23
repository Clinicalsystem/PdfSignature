using System.IO;
using System.Threading.Tasks;

namespace PdfSignature.Implementation
{
    public interface IFileManager
    {
        Task<string> Save(MemoryStream fileStream, string fileName);

    }
}
