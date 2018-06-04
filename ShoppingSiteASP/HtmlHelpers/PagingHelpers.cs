using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ShoppingSiteASP.Models;

namespace ShoppingSiteASP.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                                PagingInfo pagingInfo,
                                                Func<int,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for(int p = 1; p <= pagingInfo.TotalPages; p++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(p));
                tag.InnerHtml = p.ToString();
                if(p== pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }
                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}