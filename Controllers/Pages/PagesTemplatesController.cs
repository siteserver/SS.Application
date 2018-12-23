using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("templates")]
    public class PagesTemplatesController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var templateType = request.GetQueryString("templateType");
                var directoryPath = Context.PluginApi.GetPluginPath(ApplicationUtils.PluginId, "templates");
                var templateUrl = Context.PluginApi.GetPluginUrl(ApplicationUtils.PluginId, "templates");
                
                var templates = new List<object>();
                foreach (var directoryName in ApplicationUtils.GetDirectoryNames(directoryPath).OrderBy(x => x.Length))
                {
                    if (ApplicationUtils.StartsWithIgnoreCase(directoryName, templateType))
                    {
                        var html = TemplateManager.GetTemplateHtml(templateType, directoryName);
                        templates.Add(new
                        {
                            Id = directoryName,
                            TemplateUrl = templateUrl,
                            Html = html
                        });
                    }
                }
                
                return Ok(new
                {
                    Value = templates
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
