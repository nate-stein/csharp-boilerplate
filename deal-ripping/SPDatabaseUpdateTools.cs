using System;
using System.Collections.Generic;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Provides a utilities class with various implementations of the SPDatabaseDealAddition and 
	/// other ripping classes to call on for convenience in updating the Unit Test Database and 
	/// other ripping utilities.
	/// </summary>
	public static class SPDatabaseUpdateTools
	{
		#region AddDealFromTextFileToDatabase
		public static void AddDealFromTextFileToDatabase (
			string textFilePath,
			string databaseConnectionString)
		{
			string termSheetText = Utilities.GeneralUtils.GetStringFromTextFile (textFilePath);
			SPDatabaseDealAddition newDeal = new SPDatabaseDealAddition
			{
				DatabaseConnectionString = databaseConnectionString
			};
			newDeal.AddToDatabase (termSheetText);
		}
		#endregion

		#region AddAllDealsFromFolderToSPDatabase
		// Parses all text files in a given folder to add them to the desired database.
		public static void AddAllDealsFromFolderToSPDatabase (
			string pathToFolderContainingTextFiles,
			string databaseConnectionString,
			string filePathToSaveErrorLog)
		{
			IEnumerable<string> textFilePaths =
				SPFileManagementUtils.GetNamesOfFilesInFolder(pathToFolderContainingTextFiles);
			List<string> errorLog = new List<string> ();

			int successfulFileAdditionCount = 0;
			int failedFileAdditionCount = 0;

			foreach (string path in textFilePaths)
			{
				try
				{
					AddDealFromTextFileToDatabase (path, databaseConnectionString);
					successfulFileAdditionCount++;
				}
				catch (Exception e)
				{
					errorLog.Add ("Failed file: " + path);
					errorLog.Add ("Error: " + e);
					errorLog.Add ("");
					failedFileAdditionCount++;
				}
			}
			string resultsSummary = String.Format (
				"*** Results Summary ***\n\n" +
				"{0} files were successfully ripped.\n" +
				"{1} files encountered an exception.\n" +
				 "***********************\n\n",
				 successfulFileAdditionCount,
				 failedFileAdditionCount);

			errorLog.Insert (0, resultsSummary);
			saveErrorLog (errorLog, filePathToSaveErrorLog);
		}
		#endregion

		#region saveErrorLog
		private static void saveErrorLog (List<string> errorLog, string filePathToSaveErrorLog)
		{
			Utilities.GeneralUtils.SaveArrayToTextFile (errorLog.ToArray (), filePathToSaveErrorLog);
		}
		#endregion

		#region AddCusipFromUnitTestFolderToTestDatabase
		public static void AddCusipFromUnitTestFolderToTestDatabase (string cusip)
		{
			string filePath =
				SPFileManagementUtils.GetPathToTextFileInUnitTestFolder(cusip);
			AddDealFromTextFileToDatabase (
				filePath, SPFileManagementUtils.ConnectionStringSPTestDatabase);
		}
		#endregion

		#region DeleteCusipRecordFromSPDatabase
		/// <summary>
		/// Generic method to delete a Cusip from all passed tables in an SP Database.
		/// </summary>
		/// <param name="cusip"></param>
		/// <param name="tablesToDeleteCusipFrom"></param>
		/// <param name="databaseConnectionString"></param>
		public static void DeleteCusipRecordFromSPDatabase (
			string cusip,
			IEnumerable<string> tablesToDeleteCusipFrom,
			string databaseConnectionString)
		{
			foreach (var table in tablesToDeleteCusipFrom)
			{
				string sqlDeletionQuery = String.Format ("DELETE FROM {0} WHERE Cusip = '{1}'", table, cusip);
				try
				{
					DatabaseUpdater.ExecuteSqlQuery (sqlDeletionQuery, databaseConnectionString);
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine (e.ToString ());
				}
			}
		}
		#endregion

		#region DeleteCusipRecordFromSPTestDatabase
		public static void DeleteCusipRecordFromSPTestDatabase (string cusip)
		{
			List<string> listOfTablesToDeleteCusipFrom = SPDatabaseUtils.GetSPDatabaseListOfDealTables ();
			string connectionString = SPFileManagementUtils.ConnectionStringSPTestDatabase;
			DeleteCusipRecordFromSPDatabase (cusip, listOfTablesToDeleteCusipFrom, connectionString);
		}
		#endregion

		#region ClearAllDealDataFromMSWMDatabase
		public static void ClearAllDealDataFromMSWMDatabase ()
		{
			List<string> tablesToClear = SPDatabaseUtils.GetSPDatabaseListOfDealTables ();
			string connString = SPFileManagementUtils.ConnectionStringMSWMDatabase;
			DeleteAllRecordsFromCertainTablesInSPDatabase (tablesToClear, connString);
		}
		#endregion

		#region DeleteAllRecordsFromCertainTablesInSPDatabase
		public static void DeleteAllRecordsFromCertainTablesInSPDatabase (
			IEnumerable<string> tables,
			string dbConnectionString)
		{
			foreach (string tbl in tables)
			{
				string sqlQuery = string.Format ("DELETE FROM {0}", tbl);
				DatabaseUpdater.ExecuteSqlQuery (sqlQuery, dbConnectionString);
			}
		}
		#endregion
	}
}
