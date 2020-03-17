using System.Threading.Tasks;

namespace MyCensus.Services
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        void LongAlert(string message);
        void ShortAlert(string message);
        Task<bool> ShowConfirmAsync(string message, string title = null, string okText = null, string cancelText = null);
    }
}
