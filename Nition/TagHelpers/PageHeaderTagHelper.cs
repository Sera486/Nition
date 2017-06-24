using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Nition.TagHelpers
{
    public class PageHeaderTagHelper:TagHelper
    {
        public string Content { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }
            
            string rez =
                "<div class='container-fluid' id='gap'>"+
                    "<div class='col-md-12' id='gap-in'>"+
                        $"<h1>{Content}</h1>"+
                    "</div>"+
                "</div>";
            output.Content.SetHtmlContent(rez);
        }
    }
}
