using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Model;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/add")]
    public class PagesAddController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var settings = ApplicationUtils.GetSettings(siteId);
                var dataInfo = new DataInfo();
                var departmentInfoList = DepartmentManager.GetDepartmentInfoList(siteId);

                return Ok(new
                {
                    Value = dataInfo,
                    DepartmentInfoList = departmentInfoList,
                    Settings = settings
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
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var provideType = request.GetPostObject<List<string>>("provideType");
                var obtainType = request.GetPostObject<List<string>>("obtainType");
                var departmentId = request.GetPostInt("departmentId");
                var departmentInfo = DepartmentManager.GetDepartmentInfo(siteId, departmentId);

                var dataInfo = new DataInfo
                {
                    Id = 0,
                    SiteId = siteId,
                    AddDate = DateTime.Now,
                    QueryCode = StringUtils.GetShortGuid(true),
                    DepartmentId = departmentInfo?.Id ?? 0,
                    IsCompleted = false,
                    State = DataState.New.Value,
                    DenyReason = string.Empty,
                    RedoComment = string.Empty,
                    ReplyContent = string.Empty,
                    IsReplyFiles = false,
                    ReplyDate = DateTime.Now,
                    IsOrganization = request.GetPostBool("isOrganization"),
                    CivicName = request.GetPostString("civicName"),
                    CivicOrganization = request.GetPostString("civicOrganization"),
                    CivicCardType = request.GetPostString("civicCardType"),
                    CivicCardNo = request.GetPostString("civicCardNo"),
                    CivicPhone = request.GetPostString("civicPhone"),
                    CivicPostCode = request.GetPostString("civicPostCode"),
                    CivicAddress = request.GetPostString("civicAddress"),
                    CivicEmail = request.GetPostString("civicEmail"),
                    CivicFax = request.GetPostString("civicFax"),
                    OrgName = request.GetPostString("orgName"),
                    OrgUnitCode = request.GetPostString("orgUnitCode"),
                    OrgLegalPerson = request.GetPostString("orgLegalPerson"),
                    OrgLinkName = request.GetPostString("orgLinkName"),
                    OrgPhone = request.GetPostString("orgPhone"),
                    OrgPostCode = request.GetPostString("orgPostCode"),
                    OrgAddress = request.GetPostString("orgAddress"),
                    OrgEmail = request.GetPostString("orgEmail"),
                    OrgFax = request.GetPostString("orgFax"),
                    Title = request.GetPostString("title"),
                    Content = request.GetPostString("content"),
                    Purpose = request.GetPostString("purpose"),
                    IsApplyFree = request.GetPostBool("isApplyFree"),
                    ProvideType = TranslateUtils.ObjectCollectionToString(provideType),
                    ObtainType = TranslateUtils.ObjectCollectionToString(obtainType),
                    DepartmentName = departmentInfo == null ? string.Empty : departmentInfo.DepartmentName
                };

                Main.DataRepository.Insert(dataInfo);

                return Ok(new
                {
                    Value = dataInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
