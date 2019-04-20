﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core.Model;
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
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var dataId = request.GetQueryInt("dataId");
                
                var dataInfo = Main.DataRepository.GetDataInfo(dataId);
                
                IList<FileInfo> fileInfoList = new List<FileInfo>();
                if (dataInfo.IsReplyFiles)
                {
                    fileInfoList = Main.FileRepository.GetFileInfoList(siteId, dataId);
                }

                var logInfoList = Main.LogRepository.GetLogInfoList(siteId, dataId);
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
