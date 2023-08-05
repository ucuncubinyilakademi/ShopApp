using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace ETICARET.WEBUI.TagHelpers
{
    [HtmlTargetElement("div",Attributes ="page-model")]
    public class PageLinkTagHelper:TagHelper
    {
        public PageInfo PageModel { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<ul class='pagination'>");

            for (int i = 1; i <= PageModel.TotalPages(); i++)
            {
                stringBuilder.AppendFormat("<li class='page-item {0}'>", i == PageModel.CurrentPage ? "active " : "");
                if (string.IsNullOrEmpty(PageModel.CurrentCategory))
                {
                    stringBuilder.AppendFormat("<a href='/products?page={0}' class='page-link'>{0}</a>", i);
                }
                else
                {
                    stringBuilder.AppendFormat("<a href='/products/{1}?page={0}' class='page-link'>{0}</a>", i,PageModel.CurrentCategory);
                }
                stringBuilder.Append("</li>");
            }
            stringBuilder.Append("</ul>");
            output.Content.SetHtmlContent(stringBuilder.ToString());
            base.Process(context, output);
        }
    }
}
