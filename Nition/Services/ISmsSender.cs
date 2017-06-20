using System.Threading.Tasks;

namespace Nition.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
