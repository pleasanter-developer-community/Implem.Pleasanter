﻿using Implem.DefinitionAccessor;
using Implem.IRds;
using Implem.Libraries.DataSources.SqlServer;
using Implem.Libraries.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace Implem.CodeDefiner.Functions.Rds.Parts.MySql
{
    internal static class MySqlIndexes
    {
        internal static void General(
            ISqlObjectFactory factory,
            string generalTableName,
            string sourceTableName,
            List<IndexInfo> tableIndexCollection)
        {
            var needChangePk = Def.ColumnDefinitionCollection.Any(o =>
                o.TableName == generalTableName &&
                o.PkMySql > 0);
            if (needChangePk)
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Pk,
                    name: "Pk",
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.PkMySql > 0)
                        .Select(o => new IndexInfo.Column(
                            o.ColumnName, o.PkMySql, o.PkOrderBy))
                        .ToList()));
            }
            else if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Pk > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Pk,
                    name: "Pk",
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Pk > 0)
                        .OrderBy(o => o.Pk)
                        .Select(o => new IndexInfo.Column(
                            o.ColumnName, o.Pk, o.PkOrderBy))
                        .ToList()));
            }
            if (needChangePk)
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix1",
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Pk > 0)
                        .OrderBy(o => o.Pk)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Pk, o.PkOrderBy, o.Unique))
                        .ToList()));
            }
            if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Ix1 > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix" + (needChangePk ? "2" : "1"),
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Ix1 > 0)
                        .OrderBy(o => o.Ix1)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Ix1, o.Ix1OrderBy, o.Unique))
                        .ToList()));
            }
            if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Ix2 > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix" + (needChangePk ? "3" : "2"),
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Ix2 > 0)
                        .OrderBy(o => o.Ix2)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Ix2, o.Ix2OrderBy, o.Unique))
                        .ToList()));
            }
            if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Ix3 > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix" + (needChangePk ? "4" : "3"),
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Ix3 > 0)
                        .OrderBy(o => o.Ix3)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Ix3, o.Ix3OrderBy, o.Unique))
                        .ToList()));
            }
            if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Ix4 > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix" + (needChangePk ? "5" : "4"),
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Ix4 > 0)
                        .OrderBy(o => o.Ix4)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Ix4, o.Ix4OrderBy, o.Unique))
                        .ToList()));
            }
            if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Ix5 > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Ix,
                    name: "Ix" + (needChangePk ? "6" : "5"),
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.Ix5 > 0)
                        .OrderBy(o => o.Ix5)
                        .Select(o => new IndexInfo.Column(o.ColumnName, o.Ix5, o.Ix5OrderBy, o.Unique))
                        .ToList()));
            }
        }

        internal static void History(
            ISqlObjectFactory factory,
            string generalTableName,
            string sourceTableName,
            List<IndexInfo> tableIndexCollection)
        {
            var needChangePk = Def.ColumnDefinitionCollection.Any(o =>
                o.TableName == generalTableName &&
                o.PkMySql > 0);
            if (needChangePk)
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Pk,
                    name: "PkHistory",
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.PkHistoryMySql > 0)
                        .OrderBy(o => o.PkHistoryMySql)
                        .Select(o => new IndexInfo.Column(
                            o.ColumnName, o.PkHistoryMySql, o.PkHistoryOrderBy))
                        .ToList()));
            }
            else if (Def.ColumnDefinitionCollection.Any(o => o.TableName == generalTableName && o.Pk > 0))
            {
                tableIndexCollection.Add(new IndexInfo(
                    factory: factory,
                    tableName: sourceTableName,
                    type: IndexInfo.Types.Pk,
                    name: "PkHistory",
                    columnCollection: Def.ColumnDefinitionCollection
                        .Where(o => o.TableName == generalTableName)
                        .Where(o => o.History > 0)
                        .Where(o => o.PkHistory > 0)
                        .OrderBy(o => o.PkHistory)
                        .Select(o => new IndexInfo.Column(
                            o.ColumnName, o.PkHistory, o.PkHistoryOrderBy))
                        .ToList()));
            }
        }

        private static IEnumerable<DataRow> Get(ISqlObjectFactory factory, string sourceTableName)
        {
            return Def.SqlIoByAdmin(factory: factory).ExecuteTable(
                factory: factory,
                commandText: Def.Sql.Indexes
                    .Replace("#TableName#", sourceTableName)
                    .Replace("#InitialCatalog#", Environments.ServiceName))
                        .AsEnumerable();

        }

        internal static bool HasChangesPkIx(
            ISqlObjectFactory factory,
            string generalTableName,
            string sourceTableName,
            Sqls.TableTypes tableType)
        {
            bool PkHasChange(
                IEnumerable<IndexInfo> defIndexColumnCollection,
                IEnumerable<DataRow> dbIndexColumnCollection)
            {
                return defIndexColumnCollection
                    .Where(o => o.IndexName().StartsWith("Pk"))
                    .FirstOrDefault()
                    .IndexInfoString() != dbIndexColumnCollection
                        .Where(o => o["Name"].ToString() == "PRIMARY")
                        .OrderBy(o => o["No"].ToInt())
                        .Select(o => o["ColumnName"] + "," + o["OrderType"].ToString())
                        .Join(",");
            }
            bool IxHasChange(
                IEnumerable<IndexInfo> defIndexColumnCollection,
                IEnumerable<DataRow> dbIndexColumnCollection)
            {
                return defIndexColumnCollection
                    .Where(o => !o.IndexName().StartsWith("Pk"))
                    .Select(o => o.IndexName())
                    .OrderBy(o => o)
                    .Join(",") != dbIndexColumnCollection
                        .Where(o => o["Name"].ToString() != "PRIMARY")
                        .Where(o => o["Name"].ToString() != "ftx")
                        .Select(o => o["Name"].ToString())
                        .Distinct()
                        .OrderBy(o => o)
                        .Join(",");
            }
            if (Parameters.Rds.DisableIndexChangeDetection) return false;
            var defIndexColumnCollection = Indexes.IndexInfoCollection(
                factory: factory,
                generalTableName: generalTableName,
                sourceTableName: sourceTableName,
                tableType: tableType);
            var dbIndexColumnCollection = Get(
                factory: factory,
                sourceTableName: sourceTableName);
            return PkHasChange(defIndexColumnCollection: defIndexColumnCollection,
                dbIndexColumnCollection: dbIndexColumnCollection) ||
                    IxHasChange(defIndexColumnCollection: defIndexColumnCollection,
                        dbIndexColumnCollection: dbIndexColumnCollection);
        }
    }
}
