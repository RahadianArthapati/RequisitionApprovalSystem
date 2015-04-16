using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data;
using Mono.Data.Sqlite;
using Android.Util;

namespace RQNApprovalSystem
{
	public class cdal
	{
		public void vGetRecordSQL(string sSQL,ref SqliteDataReader rs)
		{
			SqliteCommand cmd = new SqliteCommand();
			cmd.Connection = cd.GetConnection();
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;
			rs = cmd.ExecuteReader();
		}

		public void vExecuteSQL(string sSQL)
		{
			SqliteCommand cmd = new SqliteCommand();
			cmd.Connection = cd.GetConnection();
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;
			cmd.ExecuteNonQuery();
		}
		public void vPrepareParameter(string sSQL, List<cArrayList> arr)
		{	
			SqliteCommand cmd = new SqliteCommand();
			cmd.Connection = cd.GetConnection();
			cmd.CommandText = sSQL;
			cmd.CommandType = CommandType.Text;

			for (int i = 0; i < arr.Count; i++)
			{
				cmd.Parameters.AddWithValue(arr[i].paramid, arr[i].ParamValue);
			}
			cmd.ExecuteNonQuery();
		}
	}
}

