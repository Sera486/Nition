using Microsoft.AspNetCore.Razor.TagHelpers;
using Nition.Models;

namespace Nition.TagHelpers
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
                $@"<div class='col-md-12 commentDiv row'>
                    <div class='row commentHead'>
                        <div class='commentHeadEl'><img src='/{Comment.User.ValidImageURL}' style='height:3em; width:auto;' alt='' class='img-responsive img-circle'></div>
                        <div class='commentHeadEl commentHeadElName'>{Comment.User.FullName}</div>
                        <div class='commentHeadEl commentHeadElTime'>{Comment.Date}</div>
                    </div>
                     <hr class='commentHrCenter'>
                    {(CanDelete ? $@"<button class='deleteCommentButton pull-right' style='opacity:0.5; border:none;' commentID='{Comment.ID}'>
                                   <span class='glyphicon glyphicon-trash'></span>
                                 </button>":"") }
                        <div class='row'>
                            <p class='comment'>{Comment.Text}</p>
                        </div>
                    <hr class='commentHr'>
                </div>";
            output.Content.SetHtmlContent(commentContent);
        }
    }
}
