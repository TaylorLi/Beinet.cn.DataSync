﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Beinet.cn.DataSync
{
    public static class SqlHelper
    {
        public static SqlDataReader ExecuteReader(string connstr, string sql, int timeout, params SqlParameter[] parameters)
        {
            var conn = new SqlConnection(connstr);
            var command = conn.CreateCommand();
            command.CommandText = sql;
            command.CommandTimeout = timeout;
            if (parameters != null)
                command.Parameters.AddRange(parameters);
            conn.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static object ExecuteScalar(string connstr, string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(connstr))
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                conn.Open();
                return command.ExecuteScalar();
            }
        }

        public static int ExecuteNonQuery(string connstr, string sql, params SqlParameter[] parameters)
        {
            using (var conn = new SqlConnection(connstr))
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                conn.Open();
                return command.ExecuteNonQuery();
            }
        }

        public static List<string> GetColNames(SqlDataReader reader)
        {
            List<string> ret = new List<string>();
            if (reader == null || !reader.HasRows)
                return ret;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                ret.Add(reader.GetName(i));
            }
            return ret;
        }

        public static string GetCreateSql(SqlDataReader reader, string tbName)
        {
            if (reader == null)
                return null;
            StringBuilder sb = new StringBuilder("Create table [" + tbName + "](");
            DataTable dt = reader.GetSchemaTable();
            if (dt == null || dt.Rows.Count <= 0)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                string type = row["DataTypeName"].ToString().ToLower();
                sb.Append("[" + row["ColumnName"] + "] " + type);
                switch (type)
                {
                    case "bigint":
                    case "bit":
                    case "datetime":
                    case "float":
                    case "image":
                    case "int":
                    case "money":
                    case "ntext":
                    case "real":
                    case "smalldatetime":
                    case "smallint":
                    case "smallmoney":
                    case "sql_variant":
                    case "text":
                    case "timestamp":
                    case "tinyint":
                    case "uniqueidentifier":
                    case "xml":
                        break;
                    case "decimal":
                    case "numeric":
                        sb.Append("(" + row["NumericPrecision"] + ", " + row["NumericScale"] + ")");
                        break;
                    case "binary":
                    case "char":
                    case "nchar":
                    case "nvarchar":
                    case "varbinary":
                    case "varchar":
                        if ((int)row["ColumnSize"] == int.MaxValue)
                            sb.Append("(max)");
                        else
                            sb.Append("(" + row["ColumnSize"] + ")");
                        break;
                }
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);// 移除最后一个逗号
            sb.Append(")");
            return sb.ToString();
        }

        public static bool ExistsTable(string connstr, string tableName)
        {
            string sql = "select count(1) from sys.objects where type='U' and name = @tbName";
            SqlParameter para = new SqlParameter("@tbName", SqlDbType.VarChar, 100) { Value = tableName };
            return (int)ExecuteScalar(connstr, sql, para) > 0;
        }

        public static void TruncateTable(string connstr, string tableName)
        {
            string sql = "Truncate Table " + tableName;
            ExecuteNonQuery(connstr, sql);
        }

        public static long GetTableRows(string connstr, string tableName)
        {
            string sql =
                "SELECT a.rowcnt FROM sysindexes a, sys.tables b WHERE a.id = b.[object_id] AND a.indid <=1 AND b.[name] = @tbName";
            SqlParameter para = new SqlParameter("@tbName", SqlDbType.VarChar, 100) { Value = tableName };
            object obj = ExecuteScalar(connstr, sql, para);
            if (obj == null || obj == DBNull.Value)
                return -1;
            return Convert.ToInt64(obj);
        }
    }
}
