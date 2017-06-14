using System.Threading.Tasks;

namespace OnlineCourses.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
