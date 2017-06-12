using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OnlineCourses.Models.LecturerViewModels
{
    public class CreateCourseViewModel
    {
        [Display(Name = "Назва курсу (заголовок):")]
        [Required(ErrorMessage = "Введіть назву курсу")]
        public string Title { get; set; }

        [Display(Name = "Опис:")]
        [Required(ErrorMessage = "Введіть опис курсу")]
        public string Description { get; set; }

        [Display(Name = "Виберіть картинку курсу:")]
        public IFormFile Image { get; set; }

        [Display(Name = "Ціна курсу:")]
        public double Price { get; set; }

        [Display(Name = "Приблизний час проходження:")]
        public double Estimate { get; set; }
    }
}
