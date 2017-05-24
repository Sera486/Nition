using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        //[Required]
        //[EmailAddress]
        [JsonProperty("email")]
        public string Email { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [JsonProperty("password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        [JsonProperty("confirmPassword")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [JsonProperty("login")]
        public string Login { get; set; }

        //[Required]
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        //[Required]
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("role")]
        public string Role { get; set; }
    }
}
