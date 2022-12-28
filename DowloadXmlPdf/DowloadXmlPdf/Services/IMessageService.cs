namespace DowloadXmlPdf.Services
{
    public interface IMessageService
    {
        Task Show(string message);

        Task<bool> QuestionAsync(string messagestring, string aceptar = "Aceptar", string cancelar = "Cancelar");

        Task<string> ShowAsync(string[] message);

        Task<string> Info(string messagestring);

        void ToastMessage(string messagestring);
    }
}
