using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Nition.TagHelpers
{

    
    public class PageHeaderTagHelper:TagHelper
    {
        public bool IsTitleOnly { get; set; } = true;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var content=await output.GetChildContentAsync();

            string rez =
                $@"<div class='container-fluid' id='gap'>
                    <div class='col-md-12' id='gap-in'>
                    {(IsTitleOnly? $"<h1>{content}</h1>" : content.ToString())}
                    </div>
                </div>";
            output.Content.SetHtmlContent(rez);
        }
    }
}
