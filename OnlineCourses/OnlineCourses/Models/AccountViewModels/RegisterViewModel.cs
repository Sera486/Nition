using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email field required")]
        [EmailAddress(ErrorMessage = "String is not a valid email address")]
        [Display(Name = "Email")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field required")]
        [Display(Name = "Password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation field required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "Confirm password")]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Login field required")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "First name field required")]
        [Display(Name = "First name")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name field required")]
        [Display(Name = "Last name")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role is not chosen")]
        [Display(Name = "Role")]
        [JsonProperty("role")]
        public string Role { get; set; }

        public List<SelectListItem> Roles;
    }
}
