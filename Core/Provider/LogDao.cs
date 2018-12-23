using System;
using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Application.Core.Model;

namespace SS.Application.Core.Provider
{
    public static class LogDao
    {
        public const string TableName = "ss_application_log";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(LogInfo.Id),
                DataType = DataType.Integer,
                IsIdentity = true,
                IsPrimaryKey = true
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.DataId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.UserId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.AddDate),
                DataType = DataType.DateTime
            },
            new TableColumn
            {
                AttributeName = nameof(LogInfo.Summary),
                DataType = DataType.VarChar,
                DataLength = 200
            }
        };

        public static void Insert(LogInfo logInfo)
        {
            var sqlString = $@"INSERT INTO {TableName}
(
    {nameof(LogInfo.SiteId)},
    {nameof(LogInfo.DataId)},
    {nameof(LogInfo.UserId)},
    {nameof(LogInfo.AddDate)},
    {nameof(LogInfo.Summary)}
) VALUES (
    @{nameof(LogInfo.SiteId)},
    @{nameof(LogInfo.DataId)},
    @{nameof(LogInfo.UserId)},
    @{nameof(LogInfo.AddDate)},
    @{nameof(LogInfo.Summary)}
)";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(LogInfo.SiteId), logInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.DataId), logInfo.DataId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.UserId), logInfo.UserId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.AddDate), logInfo.AddDate),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.Summary), logInfo.Summary)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void Update(LogInfo logInfo)
        {
            var sqlString = $@"UPDATE {TableName} SET
    {nameof(LogInfo.SiteId)} = @{nameof(LogInfo.SiteId)},
    {nameof(LogInfo.DataId)} = @{nameof(LogInfo.DataId)},
    {nameof(LogInfo.UserId)} = @{nameof(LogInfo.UserId)},
    {nameof(LogInfo.AddDate)} = @{nameof(LogInfo.AddDate)},
    {nameof(LogInfo.Summary)} = @{nameof(LogInfo.Summary)}
WHERE Id = @Id";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(LogInfo.SiteId), logInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.DataId), logInfo.DataId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.UserId), logInfo.UserId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.AddDate), logInfo.AddDate),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.Summary), logInfo.Summary),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.Id), logInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void DeleteByDataId(int dataId)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(LogInfo.DataId)} = {dataId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static List<LogInfo> GetLogInfoList(int siteId, int dataId)
        {
            var logInfoList = new List<LogInfo>();

            var sqlString = $@"SELECT {nameof(LogInfo.Id)},
            {nameof(LogInfo.SiteId)},
            {nameof(LogInfo.DataId)},
            {nameof(LogInfo.UserId)},
            {nameof(LogInfo.AddDate)},
            {nameof(LogInfo.Summary)}
            FROM {TableName} 
            WHERE {nameof(LogInfo.SiteId)} = @{nameof(LogInfo.SiteId)} AND {nameof(LogInfo.DataId)} = @{nameof(LogInfo.DataId)} ORDER BY Id";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(LogInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(LogInfo.DataId), dataId)
            };

            using (var rdr =
                Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    logInfoList.Add(GetLogInfo(rdr));
                }

                rdr.Close();
            }

            return logInfoList;
        }

        private static LogInfo GetLogInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;

            var logInfo = new LogInfo();

            var i = 0;
            logInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            logInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            logInfo.DataId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            logInfo.UserId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            logInfo.AddDate = rdr.IsDBNull(i) ? DateTime.Now : rdr.GetDateTime(i);
            i++;
            logInfo.Summary = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);

            if (logInfo.UserId > 0)
            {
                var adminInfo = Context.AdminApi.GetAdminInfoByUserId(logInfo.UserId);
                if (adminInfo != null)
                {
                    logInfo.UserName = adminInfo.UserName;
                }
            }

            return logInfo;
        }
    }
}
