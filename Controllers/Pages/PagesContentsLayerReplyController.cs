using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Model;
using SS.Application.Core.Provider;
using SS.Application.Core.Utils;

namespace SS.Application.Controllers.Pages
{
    [RoutePrefix("pages/contentsLayerReply")]
    public class PagesContentsLayerReplyController : ApiController
    {
        private const string Route = "{contentId:int}";
        private const string RouteUpload = "actions/upload";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig(int contentId)
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var contentInfo = DataDao.GetDataInfo(contentId);
                if (contentInfo == null || contentInfo.State != DataState.Accepted.Value &&
                    contentInfo.State != DataState.Redo.Value &&
                    contentInfo.State != DataState.Replied.Value) return NotFound();

                var fileInfoList = new List<FileInfo>();
                if (contentInfo.IsReplyFiles)
                {
                    fileInfoList = FileDao.GetFileInfoList(siteId, contentId);
                }

                return Ok(new
                {
                    Value = contentInfo,
                    FileInfoList = fileInfoList
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(RouteUpload)]
        public IHttpActionResult Upload()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var httpRequest = HttpContext.Current.Request;
                var fileName = httpRequest["fileName"];
                var fileCount = httpRequest.Files.Count;
                if (fileCount == 0) return NotFound();

                var file = httpRequest.Files[0];

                if (string.IsNullOrEmpty(fileName)) fileName = System.IO.Path.GetFileName(file.FileName);

                var filePath = Context.UtilsApi.GetUploadFilePath(siteId, fileName);
                file.SaveAs(filePath);

                var fileInfo = new FileInfo
                {
                    FileName = fileName,
                    FileUrl = Context.UtilsApi.GetUploadFileUrl(siteId, fileName),
                    Length = file.ContentLength
                };

                return Ok(new
                {
                    Value = true,
                    FileInfo = fileInfo
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Value = false,
                    ex.Message
                });
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit(int contentId)
        {
            try
            {
                var request = Context.GetCurrentRequest();
                var siteId = request.GetQueryInt("siteId");
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSitePermissions(siteId, ApplicationUtils.PluginId)) return Unauthorized();

                var replyContent = request.GetPostString("replyContent");
                var fileInfoList = request.GetPostObject<List<FileInfo>>("fileInfoList");

                var fileInfoListDatabase = FileDao.GetFileInfoList(siteId, contentId);

                DataDao.UpdateStateAndReply(siteId, contentId, replyContent, fileInfoList.Count > 0);

                LogManager.Reply(siteId, contentId, request.AdminId);

                foreach (var fileInfo in fileInfoList)
                {
                    if (fileInfo.Id == 0)
                    {
                        fileInfo.SiteId = siteId;
                        fileInfo.DataId = contentId;
                        FileDao.Insert(fileInfo);
                    }
                }

                foreach (var fileInfoDatabase in fileInfoListDatabase)
                {
                    if (fileInfoList.Exists(f => f.Id == fileInfoDatabase.Id)) continue;

                    var filePath = Context.UtilsApi.GetUploadFilePath(siteId, fileInfoDatabase.FileName);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                    FileDao.Delete(fileInfoDatabase.Id);
                }

                return Ok(new
                {
                    Value = true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
