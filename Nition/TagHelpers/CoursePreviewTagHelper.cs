using Microsoft.AspNetCore.Razor.TagHelpers;
using Nition.Models;

namespace Nition.TagHelpers
{
    public class CoursePreviewTagHelper:TagHelper
    {
        public Course Course { get; set; }
        public bool ShowStatus { get; set; } = false;
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            
            string coursePreviewContent = 
                 $@"<div class='container col-md-12 col-lg-8 col-lg-offset-2' id='course-one'>
                            <div class='col-md-4 col-sm-12' id='img-prjDiv' class='img-responsive'>
                                <img src=\'/{Course.ValidImageURL}\' alt='prjImg' id='img-prj' class='img-responsive'>
                            </div>
                            <div class='col-md-8 col-sm-12'>
                                <a href='/Course/{Course.ID}'><strong><h2 style='white-space: nowrap; overflow: hidden; text-overflow: ellipsis;'>{Course.Title}</h2></strong></a>
                                <h4>Автор: {Course.Author.FullName}</h4>
                                {(ShowStatus?$"<h5>Статус:{Course.PublishStatus}</h5>":"")}
                                <p style='white-space: nowrap; overflow: hidden; text-overflow: ellipsis;' class='text-left'>{Course.Description}</p>
                            </div>
                       </div>";
            output.Content.SetHtmlContent(coursePreviewContent);
        }
    }
}
