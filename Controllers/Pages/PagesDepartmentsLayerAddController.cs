using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Model;
using SS.Application.Core.Provider;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/departmentsLayerAdd")]
    public class PagesDepartmentsLayerAddController : ApiController
    {
        private const string Route = "";
        private const string RouteId = "{id:int}";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var departmentInfo = new DepartmentInfo();

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    if (permissions.HasSitePermissions(siteId, ApplicationUtils.PluginId))
                    {
                        allUserNames.Add(userName);
                    }
                }

                return Ok(new
                {
                    Value = departmentInfo,
                    allUserNames
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route(RouteId)]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, id);

                var allUserNames = new List<string>();
                foreach (var userName in Context.AdminApi.GetUserNameList())
                {
                    var permissions = Context.AdminApi.GetPermissions(userName);
                    if (permissions.IsSiteAdmin(siteId)) continue;

                    if (permissions.HasSitePermissions(siteId, ApplicationUtils.PluginId))
                    {
                        allUserNames.Add(userName);
                    }
                }

                return Ok(new
                {
                    Value = departmentInfo,
                    allUserNames
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Insert()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var departmentInfo = new DepartmentInfo
                {
                    Id = 0,
                    SiteId = siteId,
                    DepartmentName = request.GetPostString("departmentName"),
                    UserNames = request.GetPostString("userNames"),
                    Taxis = request.GetPostInt("taxis")
                };

                departmentInfo.Id = DepartmentDao.Insert(departmentInfo);

                return Ok(new
                {
                    Value = departmentInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut, Route(RouteId)]
        public IHttpActionResult Update(int id)
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, id);
                departmentInfo.DepartmentName = request.GetPostString("departmentName");
                departmentInfo.UserNames = request.GetPostString("userNames");
                departmentInfo.Taxis = request.GetPostInt("taxis");

                DepartmentDao.Update(departmentInfo);

                return Ok(new
                {
                    Value = departmentInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
