using System.Threading.Tasks;

namespace OnlineCourses.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
