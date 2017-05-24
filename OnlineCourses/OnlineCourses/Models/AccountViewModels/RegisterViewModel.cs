using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email field required")]
        [EmailAddress(ErrorMessage = "String is not a valid email address")]
        [JsonProperty("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field required")]
        [JsonProperty("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Password confirmation field required")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [JsonProperty("confirmPassword")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Login field required")]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "First name field required")]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name field required")]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Role is not chosen")]
        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
