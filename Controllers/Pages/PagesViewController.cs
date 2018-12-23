using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core.Model;
using SS.Application.Core.Provider;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/view")]
    public class PagesViewController : ApiController
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

                var dataId = request.GetQueryInt("dataId");
                
                var dataInfo = DataDao.GetDataInfo(dataId);
                
                var fileInfoList = new List<FileInfo>();
                if (dataInfo.IsReplyFiles)
                {
                    fileInfoList = FileDao.GetFileInfoList(siteId, dataId);
                }

                var logInfoList = LogDao.GetLogInfoList(siteId, dataId);
                var settings = ApplicationUtils.GetSettings(siteId);

                return Ok(new
                {
                    Value = dataInfo,
                    LogInfoList = logInfoList,
                    FileInfoList = fileInfoList,
                    Settings = settings
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
