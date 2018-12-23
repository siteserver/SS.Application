using System.Collections.Generic;
using System.Data;
using SiteServer.Plugin;
using SS.Application.Core.Model;

namespace SS.Application.Core.Provider
{
    public static class FileDao
    {
        public const string TableName = "ss_application_file";

        public static List<TableColumn> Columns => new List<TableColumn>
        {
            new TableColumn
            {
                AttributeName = nameof(FileInfo.Id),
                DataType = DataType.Integer,
                IsIdentity = true,
                IsPrimaryKey = true
            },
            new TableColumn
            {
                AttributeName = nameof(FileInfo.SiteId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(FileInfo.DataId),
                DataType = DataType.Integer
            },
            new TableColumn
            {
                AttributeName = nameof(FileInfo.FileName),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(FileInfo.FileUrl),
                DataType = DataType.VarChar,
                DataLength = 200
            },
            new TableColumn
            {
                AttributeName = nameof(FileInfo.Length),
                DataType = DataType.Integer
            }
        };

        public static void Insert(FileInfo fileInfo)
        {
            var sqlString = $@"INSERT INTO {TableName}
(
    {nameof(FileInfo.SiteId)},
    {nameof(FileInfo.DataId)},
    {nameof(FileInfo.FileName)},
    {nameof(FileInfo.FileUrl)},
    {nameof(FileInfo.Length)}
) VALUES (
    @{nameof(FileInfo.SiteId)},
    @{nameof(FileInfo.DataId)},
    @{nameof(FileInfo.FileName)},
    @{nameof(FileInfo.FileUrl)},
    @{nameof(FileInfo.Length)}
)";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(FileInfo.SiteId), fileInfo.SiteId),
                Context.DatabaseApi.GetParameter(nameof(FileInfo.DataId), fileInfo.DataId),
                Context.DatabaseApi.GetParameter(nameof(FileInfo.FileName), fileInfo.FileName),
                Context.DatabaseApi.GetParameter(nameof(FileInfo.FileUrl), fileInfo.FileUrl),
                Context.DatabaseApi.GetParameter(nameof(FileInfo.Length), fileInfo.Length)
            };

            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString, parameters.ToArray());
        }

        public static void DeleteByDataId(int dataId)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(FileInfo.DataId)} = {dataId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static void Delete(int fileId)
        {
            var sqlString =
                $"DELETE FROM {TableName} WHERE {nameof(FileInfo.Id)} = {fileId}";
            Context.DatabaseApi.ExecuteNonQuery(Context.ConnectionString, sqlString);
        }

        public static List<FileInfo> GetFileInfoList(int siteId, int dataId)
        {
            var fileInfoList = new List<FileInfo>();

            var sqlString = $@"SELECT {nameof(FileInfo.Id)},
            {nameof(FileInfo.SiteId)},
            {nameof(FileInfo.DataId)},
            {nameof(FileInfo.FileName)},
            {nameof(FileInfo.FileUrl)},
            {nameof(FileInfo.Length)}
            FROM {TableName} 
            WHERE {nameof(FileInfo.SiteId)} = @{nameof(FileInfo.SiteId)} AND {nameof(FileInfo.DataId)} = @{nameof(FileInfo.DataId)} ORDER BY Id";

            var parameters = new List<IDataParameter>
            {
                Context.DatabaseApi.GetParameter(nameof(FileInfo.SiteId), siteId),
                Context.DatabaseApi.GetParameter(nameof(FileInfo.DataId), dataId)
            };

            using (var rdr =
                Context.DatabaseApi.ExecuteReader(Context.ConnectionString, sqlString, parameters.ToArray()))
            {
                while (rdr.Read())
                {
                    fileInfoList.Add(GetFileInfo(rdr));
                }

                rdr.Close();
            }

            return fileInfoList;
        }

        private static FileInfo GetFileInfo(IDataRecord rdr)
        {
            if (rdr == null) return null;

            var fileInfo = new FileInfo();

            var i = 0;
            fileInfo.Id = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            fileInfo.SiteId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            fileInfo.DataId = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);
            i++;
            fileInfo.FileName = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            fileInfo.FileUrl = rdr.IsDBNull(i) ? string.Empty : rdr.GetString(i);
            i++;
            fileInfo.Length = rdr.IsDBNull(i) ? 0 : rdr.GetInt32(i);

            return fileInfo;
        }
    }
}
