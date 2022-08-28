using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public interface IMessageService
    {
        Task ShowAsync(string message);
    }
}
