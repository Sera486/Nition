using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Обов'язкове поле")]
        [EmailAddress(ErrorMessage = "Некоректний адрес пошти")]
        [StringLength(60, MinimumLength = 4)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [StringLength(60, MinimumLength = 6)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [StringLength(60, MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "Пароль і підтвердження пароля не співпадають")]
        [Display(Name = "Підтвердження пароля")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Обов'язкове поле")]
        [StringLength(60, MinimumLength = 4)]
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
