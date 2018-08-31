using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// These classes retrieve and structure the data that is used by our tests.

	#region IDateArrayTestDataSource
	/// <summary>
	/// Interface to use whenever we need to retrieve a Dictionary containing the prospectus test 
	/// text file path and an array of dates corresponding to the verified date values for the 
	/// detail we are testing.
	/// </summary>
	public interface IDateArrayTestDataSource
	{
		Dictionary<string, DateTime []> GetTestFileAndDateArrayPairDictionary ();
	}
	#endregion

	#region INumberTestDataSource
	/// <summary>
	/// Interface to use whenever we need to retrieve a Dictionary containing the prospectus test 
	/// file path and numbers corresponding to the verified value for the detail we are testing.
	/// </summary>
	public interface INumberTestDataSource
	{
		Dictionary<string, double> GetTestFileAndNumberPairDictionary ();
	}
	#endregion

	#region AccessDatabaseCallObservationDatesSource
	public class AccessDatabaseCallObservationDatesSource : IDateArrayTestDataSource
	{
		protected const int _MAX_CALL_OBSERVATIONS_TO_CHECK = 30;

		#region GetTestFileAndDateArrayPairDictionary
		public Dictionary<string, DateTime []> GetTestFileAndDateArrayPairDictionary ()
		{
			string databaseConnectionString = SPFileManagementUtils.ConnectionStringSPTestDatabase;
			using (OleDbConnection connection = new OleDbConnection (databaseConnectionString))
			{
				string sqlQuery = getCallObservationDatesSqlQuery (_MAX_CALL_OBSERVATIONS_TO_CHECK);
				OleDbCommand command = new OleDbCommand (sqlQuery, connection);
				connection.Open ();
				OleDbDataReader autocallableDataReader = command.ExecuteReader ();
				Dictionary<string, DateTime []> testFileAndDateArrayPairs = new Dictionary<string, DateTime []> ();
				while (autocallableDataReader.Read ())
				{
					string cusip = (string)autocallableDataReader ["Cusip"];
					string filePath = TestUtils.GetTestStringFilePathForGivenCusip (cusip);
					List<DateTime> callObservationDates = new List<DateTime> ();
					bool keepLoopingThroughColumns = true;
					int i = 1;
					while (keepLoopingThroughColumns && (i <= _MAX_CALL_OBSERVATIONS_TO_CHECK))
					{
						string columnHeader = "Call_Date_" + i;
						var callObservationDate = (autocallableDataReader [columnHeader] as DateTime?) ?? null;
						if (callObservationDate != null)
						{
							callObservationDates.Add ((DateTime)callObservationDate);
						}
						else keepLoopingThroughColumns = false;
						i++;
					}
					testFileAndDateArrayPairs.Add (filePath, callObservationDates.ToArray ());
					callObservationDates.Clear ();
				}
				return testFileAndDateArrayPairs;
			}
		}
		#endregion

		#region getCallObservationDatesSqlQuery
		protected string getCallObservationDatesSqlQuery (int numberOfCallObservationDatesToCapture)
		{
			string sqlQuery = "SELECT Cusip, ";
			for (int i = 1; i <= numberOfCallObservationDatesToCapture; i++)
			{
				if (i == numberOfCallObservationDatesToCapture)
				{
					sqlQuery = sqlQuery + "Call_Date_" + i;
				}
				else sqlQuery = sqlQuery + "Call_Date_" + i + ", ";
			}
			sqlQuery = sqlQuery + " FROM CallObservationDates";
			return sqlQuery;
		}
		#endregion
	}
	#endregion

	#region AccessDatabaseNumericalStructureDetailSource
	/// <summary>
	/// Its only method returns a dictionary consisting of file path keys and corresponding numbers 
	/// that represent the file paths for the text to be parsed by some kind of parser and the 
	/// corresponding number the parser is supposed to return.
	/// Used by tests to review the accuracy of DealRippers.
	/// </summary>
	public class AccessDatabaseNumericalStructureDetailSource : INumberTestDataSource
	{
		#region Members
		protected string _desiredDetailTableColumnHeader;
		protected string _tableContainingDesiredDetail;
		protected string _additionalSqlWhereClause;
		protected string _overridenSqlQuery;
		#endregion

		#region GetTestFileAndNumberPairDictionary
		public Dictionary<string, double> GetTestFileAndNumberPairDictionary ()
		{
			string databaseConnectionString = SPFileManagementUtils.ConnectionStringSPTestDatabase;
			using (OleDbConnection connection = new OleDbConnection (databaseConnectionString))
			{
				string sqlQuery = getSqlQuery ();
				OleDbCommand command = new OleDbCommand (sqlQuery, connection);
				connection.Open ();
				OleDbDataReader dataReader = command.ExecuteReader ();
				Dictionary<string, double> testFilePathAndNumberDictionary = new Dictionary<string, double> ();
				int desiredColumnOrdinalNumber = dataReader.GetOrdinal (_desiredDetailTableColumnHeader);

				while (dataReader.Read ())
				{
					string cusip = dataReader ["Cusip"].ToString ();
					string filePath = TestUtils.GetTestStringFilePathForGivenCusip (cusip);
					double detailBeingExtracted;
					try
					{
						detailBeingExtracted = dataReader.GetDouble (desiredColumnOrdinalNumber);
					}
					catch (System.InvalidCastException)
					{
						string debugMsg = String.Format ("Unable to cast value for column '{0}' to double so attempting to cast as decimal first.", _desiredDetailTableColumnHeader);
						Debug.WriteLine (debugMsg);
						try
						{
							detailBeingExtracted = (double)dataReader.GetDecimal (desiredColumnOrdinalNumber);
						}
						catch (System.InvalidCastException)
						{
							debugMsg = String.Format ("Unable to cast value for column '{0}' to decimal first either so attempting to cast as integer now.", _desiredDetailTableColumnHeader);
							detailBeingExtracted = (double)dataReader.GetInt32 (desiredColumnOrdinalNumber);
						}
						catch (Exception e)
						{
							string errorMsg = "Unhandled exception encountered in GetTestFileAndNumberPairDictionary() " +
								 "method of AccessDatabaseNumericalStructureDetailSource class.\nSql query = " + sqlQuery + "\nError details: " + e.ToString ();
							Debug.WriteLine (errorMsg);
							throw;
						}
					}
					testFilePathAndNumberDictionary.Add (filePath, detailBeingExtracted);
				}
				return testFilePathAndNumberDictionary;
			}
		}
		#endregion

		#region getSqlQuery
		protected virtual string getSqlQuery ()
		{
			if (_overridenSqlQuery == null)
			{
				string sqlQuery = string.Format (
					"SELECT Cusip, {0} FROM {1}", 
					_desiredDetailTableColumnHeader, _tableContainingDesiredDetail);
				if (_additionalSqlWhereClause != null)
				{
					sqlQuery = sqlQuery + " WHERE " + _additionalSqlWhereClause;
				}
				return sqlQuery;
			}
			else return _overridenSqlQuery;
		}
		#endregion

		#region Properties
		public string DesiredDetailTableColumnHeading
		{
			set { _desiredDetailTableColumnHeader = value; }
		}

		public string NameOfTableContainingDesiredDetail
		{
			set { _tableContainingDesiredDetail = value; }
		}

		// Added to the end of the sql query as a WHERE clause
		public string AdditionalFilteringSqlSupplement
		{
			set { _additionalSqlWhereClause = value; }
		}

		public string OverrideDefaultSqlQueryMethod
		{
			set { _overridenSqlQuery = value; }
		}
		#endregion
	}
	#endregion

	#region DataSourceFactory
	/// <summary>
	/// Factory methods for test data used in testing DealRippers.
	/// </summary>
	public class DataSourceFactory
	{
		#region GetCallObservationDatesFromAccessDatabase
		public static IDateArrayTestDataSource GetCallObservationDatesFromAccessDatabase ()
		{
			AccessDatabaseCallObservationDatesSource accessData = 
				new AccessDatabaseCallObservationDatesSource ();
			return accessData;
		}
		#endregion

		#region GetAutocallableCouponTestData
		public static INumberTestDataSource GetAutocallableCouponPerPeriodTestData ()
		{
			AccessDatabaseNumericalStructureDetailSource dataSource = 
				new AccessDatabaseNumericalStructureDetailSource
			{
				DesiredDetailTableColumnHeading = "Coupon_Per_Period",
				NameOfTableContainingDesiredDetail = "Autocallable"
			};
			return dataSource;
		}
		#endregion

		#region GetAutocallableKnockInBarrierTestData
		public static INumberTestDataSource GetAutocallableKnockInBarrierTestData ()
		{
			AccessDatabaseNumericalStructureDetailSource dataSource = 
				new AccessDatabaseNumericalStructureDetailSource
			{
				DesiredDetailTableColumnHeading = "KI_Barrier",
				NameOfTableContainingDesiredDetail = "Autocallable"
			};
			return dataSource;
		}
		#endregion

		#region GetPLUSUpsideLeverageTestData
		public static INumberTestDataSource GetPLUSUpsideLeverageTestData ()
		{
			AccessDatabaseNumericalStructureDetailSource dataSource = 
				new AccessDatabaseNumericalStructureDetailSource
			{
				DesiredDetailTableColumnHeading = "Upside_Leverage",
				NameOfTableContainingDesiredDetail = "PLUS"
			};
			return dataSource;
		}
		#endregion

		#region GetPLUSMaxReturnTestData
		public static INumberTestDataSource GetPLUSMaxReturnTestData ()
		{
			AccessDatabaseNumericalStructureDetailSource dataSource = 
				new AccessDatabaseNumericalStructureDetailSource
			{
				DesiredDetailTableColumnHeading = "Max_Return",
				NameOfTableContainingDesiredDetail = "PLUS"
			};
			string additionalFilteringSqlQuery = "Upside_Is_Capped = TRUE";
			dataSource.AdditionalFilteringSqlSupplement = additionalFilteringSqlQuery;
			return dataSource;
		}
		#endregion
	}
	#endregion

	#region TestUtils
	public static class TestUtils
	{
		#region GetTestStringFilePathForGivenCusip
		public static string GetTestStringFilePathForGivenCusip (string cusip)
		{
			string testStringFolderPath = SPFileManagementUtils.FolderPathUnitTestTextFiles;
			string [] files = System.IO.Directory.GetFiles (testStringFolderPath);
			foreach (string file in files)
			{
				if (file.Contains (cusip)) return file;
			}
			throw new System.IO.FileNotFoundException (
				"TestUtils.GetTestStringFilePathForGivenCusip() was unable to find file containing " +
				"cusip '" + cusip + ".'");
		}
		#endregion
	}
	#endregion
}
