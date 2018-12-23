using System;
using SiteServer.Plugin;
using SS.Application.Core.Utils;

namespace SS.Application.Core
{
    public static class TemplateManager
    {
        public static string GetTemplateHtml(string templateType, string directoryName)
        {
            var htmlPath = Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, $"templates/{directoryName}/index.html");

            var html = CacheUtils.Get<string>(htmlPath);
            if (html != null) return html;

            html = ApplicationUtils.ReadText(htmlPath);
            var startIndex = html.IndexOf("<body", StringComparison.Ordinal) + 5;
            var length = html.IndexOf("</body>", StringComparison.Ordinal) - startIndex;
            html = html.Substring(startIndex, length);
            html = html.Substring(html.IndexOf('\n'));

            //            var jsPath = Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, $"assets/js/{templateType}.js");
            //            var javascript = ApplicationUtils.ReadText(jsPath);
            //            html = html.Replace(
            //                $@"<script src=""../../assets/js/{templateType}.js"" type=""text/javascript""></script>",
            //                $@"<script type=""text/javascript"">
            //{javascript}
            //</script>");
            html = html.Replace("../../", "{stl.rootUrl}/SiteFiles/plugins/SS.Application/");
            html = html.Replace("../", "{stl.rootUrl}/SiteFiles/plugins/SS.Application/templates/");

            CacheUtils.InsertHours(htmlPath, html, 1);
            return html;
        }
    }
}
