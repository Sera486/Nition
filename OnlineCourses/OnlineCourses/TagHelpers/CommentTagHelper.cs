using System;
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
    public class CommentTagHelper:TagHelper
    {
        public Comment Comment { get; set; }
        public bool CanDelete { get; set; } = true;
        public string ReturnUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            
            string commentContent =
                "<div class='col-md-12 commentDiv row'>"+
                    "<div class=\"row commentHead\">"+
                        $"<div class='commentHeadEl'><img src='/{Comment.User.ValidImageURL}' style='height:3em; width:auto;' alt='' class='img-responsive img-circle'></div>" +
                        $"<div class='commentHeadEl commentHeadElName'>{Comment.User.FullName}</div>" +
                        $"<div class='commentHeadEl commentHeadElTime'>{Comment.Date}</div>" +
                    "</div>" +
                    (CanDelete ? $"<form method='post' action=\"/Course/DeleteComment?commentID={Comment.ID}&returnUrl={ReturnUrl}\">" +
                                 "<button type='submit'>" +
                                 "  <span class='glyphicon glyphicon-trash'></span>" +
                                 "</button>" +
                                 "</form>" : "") +
                    "<hr class='commentHrCenter'>" +

                        "<div class='row'>" +
                            $"<p class='comment'>{Comment.Text}</p>" +
                        "</div>" +
                    "<hr class='commentHr'>" +
                "</div>";
            output.Content.SetHtmlContent(commentContent);
        }
    }
}
