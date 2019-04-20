﻿using System;
using SiteServer.Plugin;
using SS.Application.Core.Model;

namespace SS.Application.Core
{
    public static class LogManager
    {
        public static void NewApplication(DataInfo dataInfo)
        {
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = dataInfo.SiteId,
                DataId = dataInfo.Id,
                UserId = 0,
                AddDate = DateTime.Now,
                Summary =
                    $"{(dataInfo.IsOrganization ? dataInfo.OrgName : dataInfo.CivicName)}（{(dataInfo.IsOrganization ? "公民" : "法人/其他组织")}）提交申请"
            });
        }

        public static void Translate(int siteId, int dataId, int userId, int departmentId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}转办申请至{DepartmentManager.GetDepartmentName(siteId, departmentId)}"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）转办申请至{DepartmentManager.GetDepartmentName(siteId, departmentId)}";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Accept(int siteId, int dataId, int userId, int departmentId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}受理申请"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）受理申请";
            if (departmentId > 0)
            {
                summary += $"，转办至 {DepartmentManager.GetDepartmentName(siteId, departmentId)}";
            }
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Deny(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}拒绝受理申请"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）拒绝受理申请";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Check(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}审核通过"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）审核通过";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Redo(int siteId, int dataId, int userId, string instruction)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}要求返工：{instruction}"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）要求返工：{instruction}";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }

        public static void Reply(int siteId, int dataId, int userId)
        {
            var adminInfo = Context.AdminApi.GetAdminInfoByUserId(userId);
            var summary = adminInfo.DisplayName == adminInfo.UserName
                ? $"{adminInfo.UserName}办理申请"
                : $"{adminInfo.DisplayName}（{adminInfo.UserName}）办理申请";
            Main.LogRepository.Insert(new LogInfo
            {
                Id = 0,
                SiteId = siteId,
                DataId = dataId,
                UserId = adminInfo.Id,
                AddDate = DateTime.Now,
                Summary = summary
            });
        }
    }
}
