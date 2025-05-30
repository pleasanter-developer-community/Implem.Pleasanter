﻿using Implem.DefinitionAccessor;
using Implem.Libraries.DataSources.SqlServer;
using Implem.Libraries.Utilities;
using Implem.Pleasanter.Libraries.DataSources;
using Implem.Pleasanter.Libraries.DataTypes;
using Implem.Pleasanter.Libraries.General;
using Implem.Pleasanter.Libraries.Requests;
using Implem.Pleasanter.Libraries.Security;
using Implem.Pleasanter.Libraries.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.IO;
using System.Linq;
namespace Implem.Pleasanter.Libraries.Responses
{
    public static class FileContentResults
    {
        public static ResponseFile Download(Context context, string guid)
        {
            var dataRow = GetBinariesTable(
                context: context,
                guid: guid);
            if (dataRow == null)
            {
                return null;
            }
            switch (dataRow.String("BinaryType"))
            {
                case "Images":
                    return Bytes(
                        dataRow: dataRow,
                        thumbnail: context.QueryStrings.Bool("thumbnail"));
                case "ExportData":
                    return context.CanExport(
                        ss: SiteSettingsUtilities.Get(
                            context: context,
                            siteId: dataRow.Long("ReferenceId")),
                        site: true)
                            ? Bytes(dataRow: dataRow)
                            : null;
                default:
                    return Bytes(dataRow: dataRow);
            }
        }

        public static ContentResultInheritance DownloadByApi(Context context, string guid)
        {
            DataRow dataRow = GetBinariesTable(context, guid);
            return dataRow != null
                ? Bytes(dataRow).ToContentResult(
                    id: dataRow.Long("BinaryId"),
                    referenceId: dataRow.Long("ReferenceId"),
                    binaryType: dataRow.String("BinaryType"),
                    guid: dataRow.String("Guid"),
                    extension: dataRow.String("Extension"),
                    size: dataRow.Long("Size"),
                    creator: dataRow.Long("Creator"),
                    updator: dataRow.Long("Updator"),
                    createdTime: dataRow.DateTime("CreatedTime"),
                    updatedTime: dataRow.DateTime("UpdatedTime"))
                : ApiResults.Error(
                    context: context,
                    errorData: new ErrorData(type: Error.Types.NotFound));
        }

        public static long GetReferenceId(Context context, string guid)
        {
            var referenceId = Rds.ExecuteScalar_long(
                context: context,
                statements: new SqlStatement[]
                {
                    Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .ReferenceId(),
                        join: Rds.BinariesJoinDefault()
                            .Add(new SqlJoin(
                                tableBracket: "[Items]",
                                joinType: SqlJoin.JoinTypes.Inner,
                                joinExpression: "[Binaries].[ReferenceId]=[Items].[ReferenceId]"))
                            .Add(new SqlJoin(
                                tableBracket: "[Sites]",
                                joinType: SqlJoin.JoinTypes.Inner,
                                joinExpression: "[Items].[SiteId]=[Sites].[SiteId]")),
                        where: Rds.BinariesWhere()
                            .TenantId(context.TenantId)
                            .Guid(guid)
                            .CanRead(
                                context: context,
                                idColumnBracket: "\"Binaries\".\"ReferenceId\"",
                                _using: !context.Publish))
                });
            return referenceId;
        }

        public static Attachment LinkBinary(Context context, Attachment attachment, Column column)
        {
            DataRow dataRow = GetBinariesTable(
                context: context,
                guid: attachment.Guid);
            if (dataRow != null)
            {
                attachment.ReferenceId = dataRow.Long("ReferenceId");
                attachment.Name = dataRow.String("FileName");
                attachment.Size = dataRow.Long("Size");
                attachment.SetHashCode(
                    context: context,
                    column: column,
                    bin: dataRow.Bytes("Bin"));
            }
            return attachment;
        }

        private static DataRow GetBinariesTable(Context context, string guid)
        {
            if (guid.IsNullOrEmpty()) return null;
            var join = Rds.BinariesJoinDefault()
                .Add(new SqlJoin(
                    tableBracket: "\"Items\"",
                    joinType: SqlJoin.JoinTypes.Inner,
                    joinExpression: "\"Binaries\".\"ReferenceId\"=\"Items\".\"ReferenceId\""))
                .Add(new SqlJoin(
                    tableBracket: "\"Sites\"",
                    joinType: SqlJoin.JoinTypes.Inner,
                    joinExpression: "\"Items\".\"SiteId\"=\"Sites\".\"SiteId\""));
            return Repository.ExecuteTable(
                context: context,
                statements: new SqlStatement[]
                {
                    Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .BinaryId()
                            .ReferenceId()
                            .Guid()
                            .BinaryType()
                            .Bin()
                            .Thumbnail()
                            .FileName()
                            .ContentType()
                            .Extension()
                            .Size()
                            .Creator()
                            .Updator()
                            .CreatedTime()
                            .UpdatedTime(),
                        join: join,
                        where: Rds.BinariesWhere()
                            .TenantId(context.TenantId)
                            .Guid(guid)
                            .CanRead(
                                context: context,
                                idColumnBracket: "\"Binaries\".\"ReferenceId\"",
                                _using: !context.Publish)),
                    Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .BinaryId()
                            .ReferenceId()
                            .Guid()
                            .BinaryType()
                            .Bin()
                            .Thumbnail()
                            .FileName()
                            .ContentType()
                            .Extension()
                            .Size()
                            .Creator()
                            .Updator()
                            .CreatedTime()
                            .UpdatedTime(),
                        join: join,
                        where: Rds.BinariesWhere()
                            .TenantId(context.TenantId)
                            .Guid(guid)
                            .Add(raw: $"(\"Binaries\".\"CreatedTime\"=\"Binaries\".\"UpdatedTime\" and \"Binaries\".\"Creator\"={context.UserId})"),
                        unionType: Sqls.UnionTypes.UnionAll),
                    Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .BinaryId()
                            .ReferenceId()
                            .Guid()
                            .BinaryType()
                            .Bin()
                            .Thumbnail()
                            .FileName()
                            .ContentType()
                            .Extension()
                            .Size()
                            .Creator()
                            .Updator()
                            .CreatedTime()
                            .UpdatedTime(),
                        where: Rds.BinariesWhere()
                            .TenantId(context.TenantId)
                            .Guid(guid)
                            .BinaryType("ExportData"),
                        unionType: Sqls.UnionTypes.UnionAll),
                    Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .BinaryId()
                            .ReferenceId()
                            .Guid()
                            .BinaryType()
                            .Bin()
                            .Thumbnail()
                            .FileName()
                            .ContentType()
                            .Extension()
                            .Size()
                            .Creator()
                            .Updator()
                            .CreatedTime()
                            .UpdatedTime(),
                        where: Rds.BinariesWhere()
                            .TenantId(context.TenantId)
                            .Guid(guid)
                            .BinaryType("TenantManagementImages"),
                        unionType: Sqls.UnionTypes.UnionAll)
                })
                    .AsEnumerable()
                    .FirstOrDefault();
        }

        public static bool RefererIsTenantManagement(Context context)
        {
            if (!context.UrlReferrer.IsNullOrEmpty())
            {
                var url = new UriBuilder(context.UrlReferrer);
                var ret = url.Path.Contains("/tenants")
                    || url.Path.Contains("/syslogs")
                    || url.Path.Contains("/depts")
                    || url.Path.Contains("/groups")
                    || url.Path.Contains("/users")
                    || url.Path.Contains("/registrations");
                return ret;
            }
            return false;
        }

        private static ResponseFile Bytes(DataRow dataRow, bool thumbnail = false)
        {
            var isThumbnail = thumbnail && dataRow["Thumbnail"] != DBNull.Value;
            var contentType = dataRow.String("ContentType");
            if (isThumbnail)
            {
                contentType = contentType.IsNullOrEmpty()
                    ? "image/bmp"
                    : "image/png";
            }
            var bin = isThumbnail
                ? dataRow.Bytes("Thumbnail")
                : dataRow.Bytes("Bin");
            if (bin != null)
            {
                return new ResponseFile(
                    fileContent: new MemoryStream(bin, false),
                    fileDownloadName: dataRow.String("FileName"),
                    contentType: contentType);
            }
            else
            {
                var binaryType = dataRow.String("BinaryType") == "TenantManagementImages"
                    ? "Images"
                    : dataRow.String("BinaryType");
                return new ResponseFile(
                    fileContent: new FileInfo(
                        Path.Combine(Directories.BinaryStorage(),
                            binaryType,
                            dataRow.String("Guid"))),
                        fileDownloadName: dataRow.String("FileName"),
                    contentType: contentType);
            }
        }

        public static ResponseFile DownloadTemp(Context context, string guid)
        {
            if (Parameters.BinaryStorage.TemporaryBinaryStorageProvider == "Rds")
            {
                var dataRow = Repository.ExecuteTable(
                    context: context,
                    statements: Rds.SelectBinaries(
                        column: Rds.BinariesColumn()
                            .Guid()
                            .BinaryType()
                            .Bin()
                            .Thumbnail()
                            .FileName(),
                        where: Rds.BinariesWhere()
                            .BinaryType("Temporary")
                            .Guid(guid)))
                    .AsEnumerable()
                    .FirstOrDefault();
                return Bytes(dataRow);
            }
            else
            {
                var folderPath = Path.Combine(Path.Combine(Directories.Temp(), guid));
                var files = Directory.GetFiles(folderPath);
                return new ResponseFile(new FileInfo(files[0]), Path.GetFileName(files[0]));
            }
        }

        public static FileContentResult ToFileContentResult(this FileContentResult content)
        {
            return new FileContentResult(content.FileContents, content.ContentType)
            {
                FileDownloadName = content.FileDownloadName
            };
        }

        public static FileStreamResult FileStreamResult(ResponseFile file)
        {
            if (file == null)
            {
                return null;
            }
            else if (file.IsFileInfo())
            {
                return new FileStreamResult(System.IO.File.OpenRead(file.FileInfo.FullName), file.ContentType)
                {
                    FileDownloadName = file.FileDownloadName
                };
            }
            else
            {
                return new FileStreamResult(file.FileContentsStream, file.ContentType)
                {
                    FileDownloadName = file.FileDownloadName
                };
            }
        }
    }
}