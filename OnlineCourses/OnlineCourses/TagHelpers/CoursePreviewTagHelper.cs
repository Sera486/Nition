﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using OnlineCourses.Models;

namespace OnlineCourses.TagHelpers
{
    public class CoursePreviewTagHelper:TagHelper
    {
        public Course Course { get; set; }
        public bool ShowStatus { get; set; } = false;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            
            string coursePreviewContent = 
                 "<div class=\"container col-md-12 col-lg-8 col-lg-offset-2\" id=\"course-one\">" +
                            "<div class=\"col-md-4 col-sm-12\" id=\"img-prjDiv\">" +
                                $"<img src=\'/{Course.ValidImageURL}\' alt=\"prjImg\" id=\"img-prj\" class=\"img-responsive\">" +
                            "</div>" +
                            "<div class=\"col-md-8 col-sm-12\">" +
                                $"<a href='/Course/{Course.ID}'><strong><h3>{Course.Title}</h3></strong></a>" +
                                $"<h6>Автор: {Course.Author.FullName}</h6>" +
                                (ShowStatus?$"<h6>Статус:{Course.PublishStatus}</h6>":"")+
                                $"<p class=\"text-left\">{Course.Description}</p>" +
                            "</div>" +
                       "</div>";
            output.Content.SetHtmlContent(coursePreviewContent);
        }
    }
}