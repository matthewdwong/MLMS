using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace MLMS.Common
{
	/// <summary>
	/// 
	/// </summary>
	public static class DatabaseHelper
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T? ToNullable<T>(object value) where T: struct
		{
			return (value == null) || (value == DBNull.Value) ? null : (T?)(T)value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static object FromNullable<T>(T? value) where T: struct
		{
			return value.HasValue ? (object)value.Value : DBNull.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T ToNull<T>(object value) where T: class
		{
			return (value == DBNull.Value) ? null : (T)value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToString(object value)
		{
			return ToNull<string>(value) ?? string.Empty;  
		}

		/// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(object value)
        {
            return (value == DBNull.Value) || (bool) value;
        }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static object FromNull<T>(T value) where T : class
		{
			return (value == null) ? DBNull.Value : (object)value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="row"></param>
		/// <returns></returns>
		public delegate T ToT<T>(DataRow row);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table"></param>
		/// <param name="toT"></param>
		/// <returns></returns>
		public static List<T> ToList<T>(DataTable table, ToT<T> toT) //where T: class
		{
			Debug.Assert(table != null, "Data table is null.");
			List<T> list = new List<T>();
			foreach (DataRow row in table.Rows)
				list.Add(toT(row));
			return list;
		}

		/// <summary>
		/// Given a list of values, this function adds the values with the specified key to the table.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="table"></param>
		/// <param name="values"></param>
		/// <param name="valueKey"></param>
		public static void AddToTableFromList<T>(DataTable table, IList<T> values, string valueKey)
		{
			foreach (T value in values)
			{
				DataRow row = table.NewRow();
				row[valueKey] = value;
				table.Rows.Add(row);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="table"></param>
		/// <param name="keyColumn"></param>
		/// <param name="toT"></param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(DataTable table, string keyColumn, ToT<TValue> toT) //where TValue : class
		{
			Debug.Assert(table != null, "Data table is null.");
			Debug.Assert(!string.IsNullOrEmpty(keyColumn), "Key column name is null or empty.");
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (DataRow row in table.Rows)
			{
				TKey key = (TKey)row[keyColumn];
				dictionary.Add(key, toT(row));
			}
			return dictionary;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="rows"></param>
		/// <param name="keyColumn"></param>
		/// <param name="toT"></param>
		/// <returns></returns>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(DataRow[] rows, string keyColumn, ToT<TValue> toT)
		{
			Debug.Assert(rows != null, "Data table is null.");
			Debug.Assert(!string.IsNullOrEmpty(keyColumn), "Key column name is null or empty.");
			Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();
			foreach (DataRow row in rows)
			{
				TKey key = (TKey)row[keyColumn];
				dictionary.Add(key, toT(row));
			}
			return dictionary;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="values"></param>
		/// <param name="addRow"></param>
		/// <returns></returns>
		public static DataRow ToRow(DataTable table, Dictionary<string, object> values, bool addRow)
		{
			DataRow row = table.NewRow();
			FillRow(row, values);
			if (addRow)
				table.Rows.Add(row);
			return row;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="table"></param>
		/// <param name="valueList"></param>
		public static void ToTable(DataTable table, List<Dictionary<string, object>> valueList)
		{
			foreach (Dictionary<string, object> entity in valueList)
				DatabaseHelper.ToRow(table, entity, true);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="row"></param>
		/// <param name="values"></param>
		public static void FillRow(DataRow row, Dictionary<string, object> values)
		{
			foreach (KeyValuePair<string, object> keyValuePair in values)
				row[keyValuePair.Key] = FromNull(keyValuePair.Value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToNull(string value)
		{
			return string.IsNullOrEmpty(value) ? null : value.Trim();
		}

		/// <summary>
		/// Merges table1 with table2 with only the distinct rows and returns the results in table1.
		/// </summary>
		/// <param name="table1"></param>
		/// <param name="table2"></param>
		/// <param name="primaryKeyColumnName"></param>
		public static void MergeTables<T>(DataTable table1, DataTable table2, string primaryKeyColumnName)
		{
			AddMissingColumns(table1, table2);

			List<T> ids = new List<T>();
			foreach (DataRow table1Row in table1.Rows)
				ids.Add((T)table1Row[primaryKeyColumnName]);

			foreach (DataRow table2Row in table2.Rows)
				if (!ids.Contains((T)table2Row[primaryKeyColumnName]))
					AddRow(table1, table2Row);
		}

		/// <summary>
		/// Merges table1 with table2 with only the distinct rows and returns the results in table1.
		/// </summary>
		/// <param name="table1"></param>
		/// <param name="table2"></param>
		/// <param name="primaryKeyColumnNames"></param>
		public static void MergeTables(DataTable table1, DataTable table2, string[] primaryKeyColumnNames)
		{
			AddMissingColumns(table1, table2);

			List<string> ids = new List<string>();
			foreach (DataRow table1Row in table1.Rows)
			{
				StringBuilder id = new StringBuilder();
				foreach (string primaryKeyColumnName in primaryKeyColumnNames)
				{
					id.Append(table1Row[primaryKeyColumnName]);
					id.Append('|');
				}
				ids.Add(id.ToString());
			}

			foreach (DataRow table2Row in table2.Rows)
			{
				StringBuilder id = new StringBuilder();
				foreach (string primaryKeyColumnName in primaryKeyColumnNames)
				{
					id.Append(table2Row[primaryKeyColumnName]);
					id.Append('|');
				}
				if (!ids.Contains(id.ToString()))
					AddRow(table1, table2Row);
			}
		}

		private static void AddMissingColumns(DataTable table1, DataTable table2)
		{
			foreach (DataColumn column in table2.Columns)
				if (!table1.Columns.Contains(column.ColumnName))
					table1.Columns.Add(column.ColumnName, column.DataType);
		}

		private static void AddRow(DataTable table, DataRow row)
		{
			DataRow newRow = table.NewRow();
			foreach (DataColumn column in table.Columns)
				if (row.Table.Columns.Contains(column.ColumnName))
					newRow[column] = row[column.ColumnName];
				else
					newRow[column] = DBNull.Value;
			table.Rows.Add(newRow);
		}
        /// <summary>
        /// Returns string from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(object obj)
        {
            if (obj == null || obj == DBNull.Value) return string.Empty;
            return Convert.ToString(obj);
        }

        /// <summary>
        /// Returns Int from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Returns Nullable Int from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? GetNullableInt(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// Returns long from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            return Convert.ToInt64(obj);
        }

        /// <summary>
        /// Returns Nullable long from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long? GetNullableLong(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return Convert.ToInt64(obj);
        }

        /// <summary>
        /// Returns Decimal from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object obj)
        {
            if (obj == null || obj == DBNull.Value) return 0;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// Returns Nullable Decimal from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? GetNullableDecimal(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return Convert.ToDecimal(obj);
        }

        /// <summary>
        /// Returns Datatime from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object obj)
        {
            return GetDateTime(obj, new DateTime());
        }

        /// <summary>
        /// Returns a DateTime Object
        /// If null, returns the default DateTime
        /// </summary>
        /// <param name="obj">Potential DateTime Object from Database</param>
        /// <param name="defaultDateTime">Default to use if null</param>
        /// <returns>The DateTime or default if null</returns>
        public static DateTime GetDateTime(object obj, DateTime defaultDateTime)
        {
            if (obj == null || obj == DBNull.Value) return defaultDateTime;
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Returns Nullable Datetime from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? GetNullableDateTime(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return Convert.ToDateTime(obj);
        }

        /// <summary>
        /// Returns Nullable TimeSpan from the object. Use when converting from DB Type to .Net Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static TimeSpan? GetNullableTimeSpan(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return (TimeSpan)obj;
        }

        /// <summary>
        /// Returns Boolean from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBoolean(object obj)
        {
            if (obj == null || obj == DBNull.Value) return false;
            return Convert.ToBoolean(obj);
        }

        /// <summary>
        /// Returns Nullable Boolean from the object. Use when converting from DB Type to .NET Type
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool? GetNullableBoolean(object obj)
        {
            if (obj == null || obj == DBNull.Value) return null;
            return Convert.ToBoolean(obj);
        }
	}
}
