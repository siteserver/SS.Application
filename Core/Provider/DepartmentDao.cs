using System.Collections.Generic;
using System.Data;
using System.Linq;
using SiteServer.Plugin;
using SS.Application.Core.Model;

namespace SS.Application.Core.Provider
{
    public static class DepartmentDao
    {
        public const string TableName = "ss_application_department";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(DepartmentInfo.Id),
                DataType = DataType.Integer,
                IsIdentity = true,
                IsPrimaryKey = true
            },
            new TableColumn
            {
                AttributeName = nameof(DepartmentInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(DepartmentInfo.DepartmentName),
                DataType = DataType.VarChar,
                DataLength = 500
            },
            new TableColumn
            {
                AttributeName = nameof(DepartmentInfo.UserNames),
                DataType = DataType.Text
            },
            new TableColumn
            {
                AttributeName = nameof(DepartmentInfo.Taxis),
                DataType = DataType.Integer
            }
        };

        public static int Insert(DepartmentInfo dataInfo)
        {
            var sqlString = $@"INSERT INTO {TableName}
(
    {nameof(DepartmentInfo.SiteId)},
    {nameof(DepartmentInfo.DepartmentName)},
    {nameof(DepartmentInfo.UserNames)},
    {nameof(DepartmentInfo.Taxis)}
) VALUES (
    @{nameof(DepartmentInfo.SiteId)},
    @{nameof(DepartmentInfo.DepartmentName)},
    @{nameof(DepartmentInfo.UserNames)},
    @{nameof(DepartmentInfo.Taxis)}
)";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.SiteId), dataInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.DepartmentName), dataInfo.DepartmentName),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.UserNames), dataInfo.UserNames),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.Taxis), dataInfo.Taxis)
            };

            var departmentId = Context.DatabaseApi.ExecuteNonQueryAndReturnId(TableName, nameof(DepartmentInfo.Id),
                Context.ConnectionString, sqlString, parameters.ToArray());

            DepartmentManager.ClearCache(dataInfo.SiteId);

            return departmentId;
        }

        public static void Update(DepartmentInfo departmentInfo)
        {
            var sqlString = $@"UPDATE {TableName} SET
    {nameof(DepartmentInfo.SiteId)} = @{nameof(DepartmentInfo.SiteId)},
    {nameof(DepartmentInfo.DepartmentName)} = @{nameof(DepartmentInfo.DepartmentName)},
    {nameof(DepartmentInfo.UserNames)} = @{nameof(DepartmentInfo.UserNames)},
    {nameof(DepartmentInfo.Taxis)} = @{nameof(DepartmentInfo.Taxis)}
WHERE Id = @Id";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.SiteId), departmentInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.DepartmentName), departmentInfo.DepartmentName),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.UserNames), departmentInfo.UserNames),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.Taxis), departmentInfo.Taxis),
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.Id), departmentInfo.Id)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());

            DepartmentManager.ClearCache(departmentInfo.SiteId);
        }

        public static void Delete(int siteId, int departmentId)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(DepartmentInfo.Id)} = {departmentId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);

            DepartmentManager.ClearCache(siteId);
        }

        public static List<DepartmentInfo> GetDepartmentInfoList(int siteId)
        {
            var list = new List<DepartmentInfo>();

            var sqlString = $@"SELECT
    {nameof(DepartmentInfo.Id)},
    {nameof(DepartmentInfo.SiteId)},
    {nameof(DepartmentInfo.DepartmentName)},
    {nameof(DepartmentInfo.UserNames)},
    {nameof(DepartmentInfo.Taxis)}
FROM {TableName} WHERE {nameof(DepartmentInfo.SiteId)} = @{nameof(DepartmentInfo.SiteId)}";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(DepartmentInfo.SiteId), siteId)
            };

            using (var rdr =
                Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    var departmentInfo = GetDepartmentInfo(rdr);
                    list.Add(departmentInfo);
                }

                rdr.Close();
            }

            return list.OrderBy(departmentInfo => departmentInfo.Taxis == 0 ? int.MaxValue : departmentInfo.Taxis)
                .ToList();
        }

        private static DepartmentInfo GetDepartmentInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;

            var dataInfo = new DepartmentInfo();

            var i = 0;
            dataInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            dataInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            dataInfo.DepartmentName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            dataInfo.UserNames = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            dataInfo.Taxis = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);

            return dataInfo;
        }
    }
}
