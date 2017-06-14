using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
