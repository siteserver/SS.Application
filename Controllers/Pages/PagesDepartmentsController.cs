using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Provider;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/departments")]
    public class PagesDepartmentsController : ApiController
    {
        private const string Route = "";
        private const string RouteId = "{id:int}";

        [HttpGet, Route(Route)]
        public IHttpActionResult List()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                return Ok(new
                {
                    Value = DepartmentManager.GetDepartmentInfoList(siteId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route(RouteId)]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                DepartmentDao.Delete(siteId, id);

                return Ok(new
                {
                    Value = DepartmentManager.GetDepartmentInfoList(siteId)
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
