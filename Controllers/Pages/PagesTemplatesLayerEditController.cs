﻿using System;
using System.Linq;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Model;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/templatesLayerEdit")]
    public class PagesTemplatesLayerEditController : ApiController
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

                var name = request.GetQueryString("name");
                var templateInfoList = TemplateManager.GetTemplateInfoList();
                var templateInfo =
                    templateInfoList.FirstOrDefault(x => ApplicationUtils.EqualsIgnoreCase(name, x.Name));

                return Ok(new
                {
                    Value = templateInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Clone()
        {
            try
            {
                var request = Context.AuthenticatedRequest;
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var nameToClone = request.GetPostString("nameToClone");
                var name = request.GetPostString("name");
                var description = request.GetPostString("description");

                var templateInfoList = TemplateManager.GetTemplateInfoList();
                var templateInfoToClone = templateInfoList.FirstOrDefault(x => ApplicationUtils.EqualsIgnoreCase(nameToClone, x.Name));
                if (templateInfoToClone == null) return NotFound();
                if (templateInfoList.Any(x => ApplicationUtils.EqualsIgnoreCase(name, x.Name)))
                {
                    return BadRequest($"标识为 {name} 的模板已存在，请更换模板标识！");
                }

                var templateInfo = new TemplateInfo
                {
                    Name = name,
                    Main = templateInfoToClone.Main,
                    Publisher = string.Empty,
                    Description = description,
                    Icon = templateInfoToClone.Icon
                };
                templateInfoList.Add(templateInfo);

                TemplateManager.Clone(nameToClone, templateInfo, templateInfoList);

                return Ok(new
                {
                    Value = templateInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        //[HttpPut, Route(Route)]
        //public IHttpActionResult Update()
        //{
        //    try
        //    {
        //        var request = Context.AuthenticatedRequest;
        //        var siteId = request.GetQueryInt("siteId");
        //        if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

        //        var templateId = request.GetPostString("templateId");
        //        var templateInfoList = TemplateManager.GetTemplateInfoList();
        //        var templateInfo =
        //            templateInfoList.FirstOrDefault(x => ApplicationUtils.EqualsIgnoreCase(templateId, x.Id));
        //        if (templateInfo == null) return NotFound();

        //        templateInfo.Description = request.GetPostString("description");
        //        templateInfo.ImageUrl = request.GetPostString("imageUrl");

        //        TemplateManager.SaveTemplateInfoList(templateInfoList);

        //        return Ok(new
        //        {
        //            Value = templateInfoList
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}
    }
}
