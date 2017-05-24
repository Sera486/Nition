using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OnlineCourses.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [JsonProperty("login")]
        public string Login { get; set; }

        [Required]
        [JsonProperty("password")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonProperty("rememberMe")]
        //[Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
