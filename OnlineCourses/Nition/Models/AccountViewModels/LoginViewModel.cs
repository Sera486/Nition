using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username/Email")]
        public string Login { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Запам'ятати?")]
        public bool RememberMe { get; set; }
    }
}
