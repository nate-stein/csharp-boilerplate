using System;
using System.Data.OleDb;
using System.Diagnostics;
using System.Collections.Generic;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Provides generic methods to execute sql queries on a database.
	/// </summary>
	public class DatabaseUpdater
	{
		#region ExecuteSqlQuery
		/// <summary>
		/// Execute sql queries on a given database (represented by a connection string). If we 
		/// encounter an exception attempting to execute our query after a connection is 
		/// successfully established, we attempt to Rollback() any changes that were successfully 
		/// made to the database.
		/// </summary>
		/// <param name="sqlQueries"></param>
		/// <param name="dbConnectionString"></param>
		public static void ExecuteSqlQuery (
			IEnumerable<string> sqlQueries,
			string dbConnectionString)
		{
			using (OleDbConnection connection = new OleDbConnection (dbConnectionString))
			{
				try
				{
					connection.Open ();
					OleDbTransaction transaction = null;
					transaction = connection.BeginTransaction ();
					foreach (string sqlQuery in sqlQueries)
					{
						Debug.WriteLine ("DatabaseUpdater.ExecuteSqlQuery() running the following " +
											  "query: " + sqlQuery);
						try
						{
							OleDbCommand command = new OleDbCommand (sqlQuery, connection);
							command.Transaction = transaction;
							int numberOfRecordsImpacted = command.ExecuteNonQuery ();
							Debug.WriteLine ("Number of records impacted: " + numberOfRecordsImpacted);
						}
						catch (OleDbException dbExc)
						{
							string errorMsg = "Exception encountered after successfully " +
													"establishing a connection to the database with the " +
													"following sql query: " + sqlQuery + ". " +
													dbExc.Message;
							string rollbackAttemptResults;
							try
							{
								transaction.Rollback ();
								rollbackAttemptResults = "SUCCESSFULLY rolled back any changes to database.";
							}
							catch (Exception e)
							{
								rollbackAttemptResults = "FAILED rolling back changes to database. " +
																 "Rollback() exception details: " + e.Message;
							}
							errorMsg = errorMsg + " " + rollbackAttemptResults;
							throw new DatabaseOperationException (errorMsg, dbExc);
						}
					}
					transaction.Commit ();
				}
				catch (OleDbException e)
				{
					string errorMsg = "Unable to establish connection to database with the " +
											"following connection string: " + dbConnectionString;
					Debug.WriteLine (errorMsg);
					throw new DatabaseOperationException (errorMsg, e);
				}
				catch (DatabaseOperationException) { throw; }
			}
		}
		#endregion

		#region ExecuteSqlQuery
		// Method used when we are only given one sql query. We simply convert it to an array for 
		// handling by the main method.
		public static void ExecuteSqlQuery (string sqlQuery, string databaseConnectionString)
		{
			string [] sqlQueryArray = { sqlQuery };
			ExecuteSqlQuery (sqlQueryArray, databaseConnectionString);
		}
		#endregion
	}
}
