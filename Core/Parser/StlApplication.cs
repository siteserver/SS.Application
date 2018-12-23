using System.Web;
using SiteServer.Plugin;
using SS.Application.Core.Utils;

namespace SS.Application.Core.Parser
{
    public static class StlApplication
    {
        public const string ElementName = "stl:application";
        
        private const string AttributeTheme = "theme";

        public static string Parse(IParseContext context)
        {
            var theme = string.Empty;

            foreach (var name in context.StlAttributes.AllKeys)
            {
                var value = context.StlAttributes[name];

                if (ApplicationUtils.EqualsIgnoreCase(name, AttributeTheme))
                {
                    theme = Context.ParseApi.ParseAttributeValue(value, context);
                }
            }

            if (string.IsNullOrEmpty(context.StlInnerHtml))
            {
                var elementId = $"iframe_{StringUtils.GetShortGuid(false)}";
                var libUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, "assets/lib/iframe-resizer-3.6.3/iframeResizer.min.js");
                var pageUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, $"templates/{theme}/index.html?siteId={context.SiteId}&apiUrl={HttpUtility.UrlEncode(Context.UtilsApi.GetApiUrl())}");

                return $@"
<iframe id=""{elementId}"" frameborder=""0"" scrolling=""no"" src=""{pageUrl}"" style=""width: 1px;min-width: 100%;""></iframe>
<script type=""text/javascript"" src=""{libUrl}""></script>
<script type=""text/javascript"">iFrameResize({{log: false}}, '#{elementId}')</script>
";
            }

            return $@"
<script>
var $apiUrl = '{Context.UtilsApi.GetApiUrl()}';
var $siteId = {context.SiteId};
</script>
{context.StlInnerHtml}
";
        }
    }
}
