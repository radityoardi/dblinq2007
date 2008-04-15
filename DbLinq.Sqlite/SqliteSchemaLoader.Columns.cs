﻿#region MIT license
////////////////////////////////////////////////////////////////////
// MIT license:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
//
// Authors:
//        Jiri George Moudry
////////////////////////////////////////////////////////////////////
#endregion

using System.Collections.Generic;
using System.Data;
using DbLinq.Util;
using DbLinq.Vendor;

namespace DbLinq.Sqlite
{
    partial class SqliteSchemaLoader
    {
        protected virtual IDataTableColumn ReadColumn(IDataReader dataReader, string table)
        {
            var column = new DataTableColumn();
            column.TableSchema = "main";
            column.TableName = table;
            column.ColumnName = dataReader.GetString(1);
            column.UnpackRawDbType(dataReader.GetString(2));
            column.FullType = dataReader.GetString(2);
            column.Nullable = dataReader.GetInt64(3) == 0;
            column.PrimaryKey = dataReader.GetInt64(5) == 1;
            column.Generated = column.PrimaryKey.Value && MapDbType(column) == typeof(int);
            return column;
        }

        public override IList<IDataTableColumn> ReadColumns(IDbConnection connectionString, string databaseName)
        {
            const string sql = @" SELECT tbl_name FROM sqlite_master WHERE type='table' order by tbl_name";
            const string pragma = @"PRAGMA table_info('{0}');";

            return Schema.DataCommand.Find<IDataTableColumn>(connectionString, sql, pragma, ReadColumn);
        }
    }
}
