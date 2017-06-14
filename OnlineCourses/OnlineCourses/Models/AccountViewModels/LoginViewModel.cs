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
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonProperty("rememberMe")]
        //[Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
