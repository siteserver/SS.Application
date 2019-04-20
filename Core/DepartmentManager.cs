﻿using System.Collections.Generic;
using System.Linq;
using SS.Application.Core.Model;
using SS.Application.Core.Utils;

namespace SS.Application.Core
{
    public static class DepartmentManager
    {
        private static class DepartmentManagerCache
        {
            private static readonly object LockObject = new object();
            private static string GetCacheKey(int siteId)
            {
                return $"SS.Application.Core.DepartmentManager.{siteId}";
            }

            public static List<DepartmentInfo> GetDepartmentInfoListByCache(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                var departmentInfoList = CacheUtils.Get<List<DepartmentInfo>>(cacheKey);
                if (departmentInfoList != null) return departmentInfoList;

                lock (LockObject)
                {
                    departmentInfoList = CacheUtils.Get<List<DepartmentInfo>>(cacheKey);
                    if (departmentInfoList == null)
                    {
                        departmentInfoList = Main.DepartmentRepository.GetDepartmentInfoList(siteId);

                        CacheUtils.InsertHours(cacheKey, departmentInfoList, 12);
                    }
                }

                return departmentInfoList;
            }

            public static void Clear(int siteId)
            {
                var cacheKey = GetCacheKey(siteId);
                CacheUtils.Remove(cacheKey);
            }
        }

        public static List<DepartmentInfo> GetDepartmentInfoList(int siteId)
        {
            return DepartmentManagerCache.GetDepartmentInfoListByCache(siteId);
        }

        public static List<int> GetDepartmentIdList(int siteId, string userName)
        {
            var departmentIdList = new List<int>();
            var departmentInfoList = DepartmentManagerCache.GetDepartmentInfoListByCache(siteId);
            foreach (var departmentInfo in departmentInfoList)
            {
                if (StringUtils.In(departmentInfo.UserNames, userName))
                {
                    departmentIdList.Add(departmentInfo.Id);
                }
            }

            return departmentIdList;
        }

        public static DepartmentInfo GetDepartmentInfo(int siteId, int departmentId)
        {
            var entries = DepartmentManagerCache.GetDepartmentInfoListByCache(siteId);

            return entries.FirstOrDefault(x => x != null && x.Id == departmentId);
        }

        public static string GetDepartmentName(int siteId, int departmentId)
        {
            var departmentInfo = GetDepartmentInfo(siteId, departmentId);
            return departmentInfo?.DepartmentName;
        }

        public static void ClearCache(int siteId)
        {
            DepartmentManagerCache.Clear(siteId);
        }
    }
}
