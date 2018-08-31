using System;

namespace Trinity.DealRipping
{
	public static class DateTimeExtensions
	{
		public static bool IsEmpty (this DateTime dateTime)
		{
			return dateTime == default (DateTime);
		}

		/// <summary>
		/// Convert a date value to the string value required by Oledb Databases.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static string ToDatabaseText (this DateTime date)
		{
			return date.ToString ("yyyy-MM-dd HH:mm:ss");
		}
	}
}
