using System.ComponentModel.DataAnnotations;

namespace Nition.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Некоректний адрес пошти")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Compare("Password", ErrorMessage = "Пароль і підтвердження пароля не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Логін")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Оберіть свою роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}
