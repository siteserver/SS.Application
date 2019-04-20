﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core.Model;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/contentsLayerDelete")]
    public class PagesContentsLayerDeleteController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var contentIdList = TranslateUtils.StringCollectionToIntList(request.GetQueryString("contentIds"));

                var dataInfoList = new List<DataInfo>();
                foreach (var contentId in contentIdList)
                {
                    var contentInfo = Main.DataRepository.GetDataInfo(contentId);
                    if (contentInfo == null) continue;

                    dataInfoList.Add(contentInfo);
                }

                return Ok(new
                {
                    Value = dataInfoList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var contentIdList = TranslateUtils.StringCollectionToIntList(request.GetPostString("contentIds"));

                foreach (var contentId in contentIdList)
                {
                    Main.DataRepository.Delete(siteId, contentId);
                    Main.LogRepository.DeleteByDataId(contentId);
                }

                return Ok(new
                {
                    Value = contentIdList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
