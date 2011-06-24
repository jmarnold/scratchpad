using FubuMVC.Core.Content;
using FubuMVC.Core.View;
using HtmlTags;

namespace Scratchpad.Web
{
    public static class TagExtensions
    {
        public static HtmlTag ImageFor(this IFubuPage page, string path)
        {
            var url = page.Get<IContentRegistry>().ImageUrl(path);
            return new HtmlTag("img").Attr("src", url);
        }
    }
}