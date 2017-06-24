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

    [HtmlTargetElement("div", Attributes = TypeAttributeName)]
    [HtmlTargetElement("div", Attributes = RouteAttributeName)]
    [HtmlTargetElement("div", Attributes = ParamsAttributeName)]
    [HtmlTargetElement("div", Attributes = OutputAttributeName)]
    public class AjaxTagHelper:TagHelper
    {
        private const string TypeAttributeName = "ajax-type";
        private const string RouteAttributeName = "ajax-route";
        private const string ParamsAttributeName = "ajax-params";
        private const string OutputAttributeName = "ajax-output";

        public enum MethodType
        {
            get,post
        }

        [HtmlAttributeName(TypeAttributeName)]
        public MethodType Type { get; set; }

        [HtmlAttributeName(RouteAttributeName)]
        public string Route { get; set; }

        [HtmlAttributeName(ParamsAttributeName)]
        public object Params { get; set; }

        [HtmlAttributeName(OutputAttributeName)]
        public string Output { get; set; }

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

            if(Route==null) {throw new InvalidOperationException();}

            string type;
            switch (Type)
            {
                case MethodType.get: type = "get";
                    break;
                    case MethodType.post: type = "post";
                    break;
                default:throw new ArgumentException($"Unknown reqest type: {Type}");
            }
            
            StringBuilder rez = new StringBuilder($"$.{type}('{Route}");

            var parameters = GetParamsString(Params);
            if (string.IsNullOrWhiteSpace(parameters))
            {
                rez.Append("'");
                
            }
            else {rez.Append(parameters+"'");}

            if (!string.IsNullOrWhiteSpace(Output))
            {
                rez.Append($",function (data){{$('{Output}').html(data);}}");
            }
            rez.Append(")");
            output.Attributes.Add("onclick", rez.ToString());
        }

        private string GetParamsString(object parameters)
        {
            StringBuilder s=new StringBuilder();
            var arr = Params.GetType().GetProperties();
            if (arr.Length != 0)
            {
                s.Append($"?{arr[0].Name}={arr[0].GetValue(parameters)}");
                for (int i = 1; i < arr.Length; i++)
                {
                    s.Append($"&{arr[i].Name}={arr[i].GetValue(parameters)}");
                }
            }
            return s.ToString();
        }
    }
}
