using System;
using System.Linq.Expressions;
using FubuCore;
using FubuMVC.Core.Content;
using FubuMVC.Core.UI;
using FubuMVC.Core.View;
using HtmlTags;

namespace Scratchpad.Web
{
    public static class TagExtensions
    {
        public static HtmlTag CreateImageFor(this IFubuPage page, string path)
        {
            var url = page.Get<IContentRegistry>().ImageUrl(path);
            return new HtmlTag("img").Attr("src", url);
        }

        public static HtmlTag ScriptFor(this IFubuPage page, string path)
        {
            return new HtmlTag("script").Attr("type", "text/javascript").Attr("src", path.ToAbsoluteUrl());
        }

        public static HtmlTag RowFor<T>(this IFubuPage<T> page, Expression<Func<T, object>> expression)
            where T : class
        {
            var row = new HtmlTag("div").AddClass("row");
            row.Append(page.LabelFor(expression));
            row.Append(page.InputFor(expression));
            return row;
        }
    }
}