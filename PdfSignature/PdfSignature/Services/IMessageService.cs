using System.Threading.Tasks;

namespace PdfSignature.Services
{
    public interface IMessageService
    {
        Task ShowAsync(string message);

        Task<bool> QuestionAsync(string messagestring, string  aceptar = "Aceptar", string cancelar ="Cancelar");

        Task<string> ShowAsync(string []message);
    }
}
