using System;

namespace RQNApprovalSystem
{
	public class Utils
	{
		private static readonly Random getrandom = new Random();
		private static readonly object syncLock = new object();
		private static int min = 1;
		private static int max = 20000;
		public static int GetRandomNumber()
		{
			lock(syncLock) { // synchronize
				return getrandom .Next(min, max);
			}
		}
		public static string GetFormattedDate(string sDate){
			if (sDate.Length == 8) {
				var year = sDate.Substring (0, 4);
				var month = sDate.Substring (4, 2);
				var day = sDate.Substring (6, 2);
				DateTime dt = new DateTime (2000, Convert.ToInt16 (month), 1);
				string sMonthName = dt.ToString ("MMMM");
				string date = string.Format ("{0} {1} {2}", day, sMonthName, year);
				return date;
			} else if (sDate.Length == 7) {
				var year = sDate.Substring (0, 4);
				var month = sDate.Substring (4, 2);
				var day = sDate.Substring (6, 1);
				DateTime dt = new DateTime (2000, Convert.ToInt16 (month), 1);
				string sMonthName = dt.ToString ("MMMM");
				string date = string.Format ("{0} {1} {2}", day, sMonthName, year);
				return date;
			} else {
				return sDate;
			}

		}
	}
}

