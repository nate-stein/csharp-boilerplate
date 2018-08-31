
namespace Trinity.DealRipping
{
	/// <summary>
	/// A central location for various methods and variables used to update or query the SP Test 
	/// Database.
	/// </summary>
	public static class SPDatabaseUtils
	{
		internal static string TableNameGeneralDetails = "GeneralDetails";
		internal static string TableNameAutocallable = "Autocallable";
		internal static string TableNameCallObservationDates = "CallObservationDates";
		internal static string TableNameCouponObservationDates = "CouponObservationDates";
		internal static string TableNameJump = "Jump";
		internal static string TableNamePLUS = "PLUS";
		internal static string TableNamePrincipalProtectedNote = "PrincipalProtectedNote";

		// Returns a list of tables that contain deal summaries that are supposed to be maintained 
		// in a Structured Products Database.
		public static System.Collections.Generic.List<string> GetSPDatabaseListOfDealTables ()
		{
			System.Collections.Generic.List<string> tablesInStructuredProductsDatabase =
				 new System.Collections.Generic.List<string> {
                TableNameAutocallable,
                TableNameCallObservationDates,
                TableNameCouponObservationDates,
                TableNameGeneralDetails,
                TableNameJump,
                TableNamePLUS,
                TableNamePrincipalProtectedNote
                };
			return tablesInStructuredProductsDatabase;
		}
	}
}
