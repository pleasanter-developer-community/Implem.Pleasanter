﻿using Implem.DefinitionAccessor;
using Implem.IRds;
using Implem.Libraries.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Implem.Libraries.DataSources.SqlServer
{
    public class SqlUpdate : SqlStatement
    {
        public SqlUpdate()
        {
        }

        public override void BuildCommandText(
            ISqlObjectFactory factory,
            SqlContainer sqlContainer,
            ISqlCommand sqlCommand,
            StringBuilder commandText,
            int? commandCount = null)
        {
            if (!Using) return;
            Build_If(commandText);
            Build_UpdateStatement(
                factory: factory,
                sqlContainer: sqlContainer,
                sqlCommand: sqlCommand,
                commandText: commandText,
                commandCount: commandCount);
            SetMainQueryInfoForSub();
            SqlWhereCollection?.BuildCommandText(
                factory: factory,
                sqlContainer: sqlContainer,
                sqlCommand: sqlCommand,
                commandText: commandText,
                commandCount: commandCount);
            AddParams_Where(
                factory: factory,
                sqlCommand: sqlCommand,
                commandCount: commandCount);
            AddParams_Param(
                factory: factory,
                sqlCommand: sqlCommand,
                commandCount: commandCount);
            AddTermination(commandText: commandText);
            Build_EndIf(commandText: commandText);
        }

        private void Build_UpdateStatement(
            ISqlObjectFactory factory,
            SqlContainer sqlContainer,
            ISqlCommand sqlCommand,
            StringBuilder commandText,
            int? commandCount)
        {
            var tableBracket = TableBracket;
            switch (TableType)
            {
                case Sqls.TableTypes.History: tableBracket = HistoryTableBracket; break;
                case Sqls.TableTypes.Deleted: tableBracket = DeletedTableBracket; break;
            }
            var columnNameCollection = new List<string>();
            if (AddUpdatorParam) columnNameCollection.Add($"\"Updator\" = {Parameters.Parameter.SqlParameterPrefix}U");
            if (AddUpdatedTimeParam) columnNameCollection.Add($"\"UpdatedTime\" = {factory.Sqls.CurrentDateTime} ");
            SqlParamCollection?
                .Where(o => (o as SqlParam).Using)
                .Where(o => (o as SqlParam).Updating)
                .ForEach(sqlParam =>
                {
                    if (!sqlParam.Raw.IsNullOrEmpty())
                    {
                        switch (sqlParam.Raw?.ToString())
                        {
                            case "@@identity":
                                columnNameCollection.Add(
                                    sqlParam.ColumnBracket + $"={Parameters.Parameter.SqlParameterPrefix}I");
                                break;
                            default:
                                columnNameCollection.Add(
                                    sqlParam.ColumnBracket + "=" + sqlParam.Raw
                                        .Replace("#CommandCount#", commandCount.ToString()));
                                break;
                        }
                    }
                    else if (sqlParam.Sub != null)
                    {
                        columnNameCollection.Add(sqlParam.ColumnBracket + "=(" +
                            sqlParam.Sub.GetCommandText(
                                factory: factory,
                                sqlContainer: sqlContainer,
                                sqlCommand: sqlCommand,
                                prefix: "_sub",
                                commandCount: commandCount) + ")");
                    }
                    else if (!sqlParam.ColumnBracket.IsNullOrEmpty())
                    {
                        columnNameCollection.Add(
                            sqlParam.ColumnBracket + "=@" + sqlParam.VariableName + commandCount);
                    }
                });
            commandText.Append("update ", tableBracket,
                " set ", columnNameCollection.Join(), " ");
        }

        private void SetMainQueryInfoForSub()
        {
            SqlWhereCollection
                .Where(o => o.Sub != null)
                .ForEach(o => o.Sub.SetMainQueryInfo(
                    sqlClass: GetType().ToString(),
                    allTableBrackets: GetAllTableBrackets()));
        }

        private void Build_CopyToHistoryStatement(
            StringBuilder commandText, string commandText_MoveToHistory, int? commandCount)
        {
            commandText.Append(commandText_MoveToHistory
                .Replace("[[CommandCount]]", commandCount.ToString()));
        }
    }
}
