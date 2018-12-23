using System.Collections.Generic;
using SiteServer.Plugin;
using SS.Application.Core;
using SS.Application.Core.Parser;
using SS.Application.Core.Provider;
using SS.Application.Core.Utils;
using Menu = SiteServer.Plugin.Menu;

namespace SS.Application
{
    public class Main : PluginBase
    {
        public override void Startup(IService service)
        {
            service
                .AddSiteMenu(siteId =>
                {
                    var request = Context.GetCurrentRequest();
                    if (!request.AdminPermissions.IsSiteAdmin(siteId))
                    {
                        var userDepartmentIdList = DepartmentManager.GetDepartmentIdList(siteId, request.AdminName);
                        if (userDepartmentIdList.Count == 0) return null;

                        var acceptCount = DataDao.GetUserCount(siteId, new List<DataState>
                        {
                            DataState.New
                        }, string.Empty, 0, userDepartmentIdList);
                        var replyCount = DataDao.GetUserCount(siteId, new List<DataState>
                        {
                            DataState.Accepted,
                            DataState.Redo
                        }, string.Empty, 0, userDepartmentIdList);

                        return new Menu
                        {
                            Text = "依申请公开",
                            IconClass = "ion-ios-book",
                            Menus = new List<Menu>
                            {
                                new Menu
                                {
                                    Text = $"待受理申请 ({acceptCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeAccept}"
                                },
                                new Menu
                                {
                                    Text = $"待办理申请 ({replyCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeReply}"
                                },
                                new Menu
                                {
                                    Text = "新增申请",
                                    Href = "pages/add.html"
                                }
                            }
                        };
                    }
                    else
                    {
                        var acceptCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.New
                        });
                        var replyCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.Accepted,
                            DataState.Redo
                        });
                        var checkCount = DataCountManager.GetCount(siteId, new List<DataState>
                        {
                            DataState.Replied
                        });
                        var totalCount = DataCountManager.GetTotalCount(siteId);

                        return new Menu
                        {
                            Text = "依申请公开",
                            IconClass = "ion-ios-book",
                            Menus = new List<Menu>
                            {
                                new Menu
                                {
                                    Text = $"待受理申请 ({acceptCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeAccept}"
                                },
                                new Menu
                                {
                                    Text = $"待办理申请 ({replyCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeReply}"
                                },
                                new Menu
                                {
                                    Text = $"待审核申请 ({checkCount})",
                                    Href = $"pages/contents.html?pageType={ApplicationUtils.PageTypeCheck}"
                                },
                                new Menu
                                {
                                    Text = $"所有申请 ({totalCount})",
                                    Href = "pages/contents.html"
                                },
                                new Menu
                                {
                                    Text = "新增申请",
                                    Href = "pages/add.html"
                                },
                                new Menu
                                {
                                    Text = "部门及权限设置",
                                    Href = "pages/departments.html"
                                },
                                new Menu
                                {
                                    Text = "依申请公开设置",
                                    Href = "pages/settings.html"
                                },
                                new Menu
                                {
                                    Text = "依申请公开模板",
                                    Href = "pages/templates.html"
                                }
                            }
                        };
                    }
                })
                .AddDatabaseTable(DataDao.TableName, DataDao.Columns)
                .AddDatabaseTable(DepartmentDao.TableName, DepartmentDao.Columns)
                .AddDatabaseTable(LogDao.TableName, LogDao.Columns)
                .AddDatabaseTable(FileDao.TableName, FileDao.Columns)
                .AddStlElementParser(StlApplication.ElementName, StlApplication.Parse)
                ;
        }
    }
}