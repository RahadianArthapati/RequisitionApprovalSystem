using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System.IO;
using System.Data;    

namespace RQNApprovalSystem
{
	public static class cd
	{
		private static SqliteConnection _cnn;

		public static SqliteConnection GetConnection()	
		{
			if (_cnn == null) 
			{
				var documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
				var pathToDatabase = System.IO.Path.Combine(documents, "InputData.db");
				var connectionString = string.Format("Data Source={0};Version=3;", pathToDatabase);
				_cnn = new SqliteConnection(connectionString);
				bool exists = File.Exists(pathToDatabase);
				if (!exists)
				{
					SqliteConnection.CreateFile(pathToDatabase);
				}
				_cnn.Open();
			}

			if (_cnn.State == ConnectionState.Closed)
				_cnn.Open();

			return (_cnn);
		}
	}
}

