using System;
using System.Collections.Generic;
using System.Diagnostics;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Coordinates all the classes and methods needed to add a new deal to the Structured Products 
	/// database.
	/// </summary>
	public class SPDatabaseDealAddition
	{
		private string _termSheetText;

		#region AddToDatabase
		/// <summary>
		/// Central method using several methods in order to add the deal represented by the term 
		/// sheet text it is given to the database.
		/// </summary>
		/// <param name="termSheetText"></param>
		public void AddToDatabase (string termSheetText)
		{
			_termSheetText = termSheetText;
			try
			{
				EStructure structureType = getStructureType ();
				string [] sqlQueries = getSqlQueriesToAddDealToDatabase (structureType);
				executeSqlQueriesToAddDealToDatabase (sqlQueries);
			}
			catch (System.Data.OleDb.OleDbException e)
			{
				string errorMsg = "Unhandled OleDbException exception encountered while trying to " +
										"update the database.\n" + e;
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (ArgumentOutOfRangeException e)
			{
				string errorMsg = "ArgumentOutOfRangeException encountered in CentralCommand. Aborting CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
			}
			catch (DealFactoryException e)
			{
				string errorMsg = "Unhandled DealFactoryException exception encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (DetailParsingException e)
			{
				string errorMsg = "Unhandled DetailParsingException exception encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (DatabaseOperationException e)
			{
				string errorMsg = "DatabaseOperationException encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (SqlQueryCreationException e)
			{
				string errorMsg = "Unhandled SqlQueryFactoryException exception encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (DealRipperFactoryException e)
			{
				string errorMsg = "Unhandled DealRipperFactoryException exception encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (System.IO.FileNotFoundException e)
			{
				string errorMsg = "File we wanted to parse wasn't found.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
			catch (Exception e)
			{
				string errorMsg = "UNKNOWN exception encountered in CentralCommand.\n" + e.ToString ();
				Debug.WriteLine (errorMsg);
				throw;
			}
		}
		#endregion

		#region getStructureType
		private EStructure getStructureType ()
		{
			IDetailRipper<EStructure> structureTypeRipper = DetailRipperFactory.GetStructureTypeRipper ();
			return structureTypeRipper.Get (ref _termSheetText);
		}
		#endregion

		#region getSqlQueriesToAddDealToDatabase
		/// <summary>
		/// Define the sql queries that will update our database for a given structure by 
		/// (1) Instantiating both the appropriate Factory and DealRipper that can be used to fill a container 
		/// that will be passed to the SqlQueryFactory class.
		/// (2) Calling the GetSqlQueriesForAddingDealToDatabase() method of SqlQueryFactory in order to get the sql
		/// queries.
		/// </summary>
		private string [] getSqlQueriesToAddDealToDatabase (EStructure structureType)
		{
			ISqlQueryGenerator sqlQueryGenerator;
			switch (structureType)
			{
				case EStructure.PrincipalProtectedNote:
					PrincipalProtectedNoteRipper ppnRipper = DealRipperFactory.GetPrincipalProtectedNoteRipper (_termSheetText);
					PrincipalProtectedNoteFactory ppnFactory = new PrincipalProtectedNoteFactory ();
					PrincipalProtectedNoteContainer ppnContainer = ppnFactory.GetDealContainer (ppnRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (ppnContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.StandardPLUS:
					PLUSRipper plusRipper = DealRipperFactory.GetPLUSRipper (_termSheetText);
					PLUSFactory plusFactory = new PLUSFactory ();
					PLUSContainer plusContainer = plusFactory.GetDealContainer (plusRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (plusContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.BufferedPLUS:
					BufferedPLUSRipper bufferedPLUSRipper = DealRipperFactory.GetBufferedPLUSRipper (_termSheetText);
					BufferedPLUSFactory bufferedPLUSFactory = new BufferedPLUSFactory ();
					BufferedPLUSContainer bufferedPLUSContainer = bufferedPLUSFactory.GetDealContainer (bufferedPLUSRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (bufferedPLUSContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.TriggerPLUS:
					TriggerPLUSRipper triggerPLUSRipper = DealRipperFactory.GetTriggerPLUSRipper (_termSheetText);
					TriggerPLUSFactory triggerPLUSFactory = new TriggerPLUSFactory ();
					TriggerPLUSContainer triggerPLUSContainer = triggerPLUSFactory.GetDealContainer (triggerPLUSRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (triggerPLUSContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.DualDirectionalTriggerPLUS:
					TriggerPLUSRipper dualDirectionalTriggerPLUSRipper = DealRipperFactory.GetTriggerPLUSRipper (_termSheetText);
					DualDirectionalTriggerPLUSFactory dualDirectionalTriggerPLUSFactory = new DualDirectionalTriggerPLUSFactory ();
					TriggerPLUSContainer dualDirectionalTriggerPLUSContainer = dualDirectionalTriggerPLUSFactory.GetDealContainer (dualDirectionalTriggerPLUSRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (dualDirectionalTriggerPLUSContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.StandardJump:
					JumpRipper jumpRipper = DealRipperFactory.GetStandardJumpRipper (_termSheetText);
					JumpFactory jumpFactory = new JumpFactory ();
					JumpContainer jumpContainer = jumpFactory.GetDealContainer (jumpRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (jumpContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.BufferedJump:
					BufferedJumpRipper bufferedJumpRipper = DealRipperFactory.GetBufferedJumpRipper (_termSheetText);
					BufferedJumpFactory bufferedJumpFactory = new BufferedJumpFactory ();
					BufferedJumpContainer bufferedJumpContainer = bufferedJumpFactory.GetDealContainer (bufferedJumpRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (bufferedJumpContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.TriggerJump:
					TriggerJumpRipper triggerJumpRipper = DealRipperFactory.GetTriggerJumpRipper (_termSheetText);
					TriggerJumpFactory triggerJumpFactory = new TriggerJumpFactory ();
					TriggerJumpContainer triggerJumpContainer = triggerJumpFactory.GetDealContainer (triggerJumpRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (triggerJumpContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.EnhancedTriggerJump:
					TriggerJumpRipper enhancedTriggerJumpRipper = DealRipperFactory.GetTriggerJumpRipper (_termSheetText);
					EnhancedTriggerJumpFactory enhancedTriggerJumpFactory = new EnhancedTriggerJumpFactory ();
					EnhancedTriggerJumpContainer enhancedTriggerJumpContainer = enhancedTriggerJumpFactory.GetDealContainer (enhancedTriggerJumpRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (enhancedTriggerJumpContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.DualDirectionalTriggerJump:
					TriggerJumpRipper dualDirectionalTriggerJumpRipper = DealRipperFactory.GetTriggerJumpRipper (_termSheetText);
					DualDirectionalTriggerJumpFactory dualDirectionalTriggerJumpFactory = new DualDirectionalTriggerJumpFactory ();
					TriggerJumpContainer dualDirectionalTriggerJumpContainer = dualDirectionalTriggerJumpFactory.GetDealContainer (dualDirectionalTriggerJumpRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (dualDirectionalTriggerJumpContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.AutocallableStandard:
					AutocallableRipper autocallableRipper = DealRipperFactory.GetAutocallableRipper (_termSheetText);
					AutocallableFactory autocallableFactory = new AutocallableFactory ();
					AutocallableContainer autocallableContainer = autocallableFactory.GetDealContainer (autocallableRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (autocallableContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.AutocallableFixedCoupon:
					FixedCouponAutocallableRipper fixedCouponAutocallableRipper = DealRipperFactory.GetFixedCouponAutocallableRipper (_termSheetText);
					AutocallableFactory fixedCouponAutocallableFactory = new AutocallableFactory ();
					AutocallableContainer fixedCouponAutocallableContainer = fixedCouponAutocallableFactory.GetDealContainer (fixedCouponAutocallableRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (fixedCouponAutocallableContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.AutocallableStepUp:
					AutocallableRipper stepUpAutocallableRipper = DealRipperFactory.GetAutocallableRipper (_termSheetText);
					StepUpAutocallableFactory stepUpAutocallableFactory = new StepUpAutocallableFactory ();
					StepUpAutocallableContainer stepUpAutocallableContainer = stepUpAutocallableFactory.GetDealContainer (stepUpAutocallableRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (stepUpAutocallableContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.AutocallableStepDown:
					AutocallableRipper stepDownAutocallableRipper = DealRipperFactory.GetAutocallableRipper (_termSheetText);
					StepDownAutocallableFactory stepDownAutocallableFactory = new StepDownAutocallableFactory ();
					StepDownAutocallableContainer stepDownAutocallableContainer = stepDownAutocallableFactory.GetDealContainer (stepDownAutocallableRipper);
					sqlQueryGenerator = SqlQueryGeneratorFactory.GetSqlQueryGenerator (stepDownAutocallableContainer);
					return sqlQueryGenerator.GetSqlQueriesForAddingDealToDatabase ();
				case EStructure.AutocallableWorstOf:
					string wOfErrorMsg = "defineSqlQueriesToAddDealToDatabase() method of CentralCommand unable to handle Worst-of Autocallables.";
					Debug.WriteLine (wOfErrorMsg);
					throw new ArgumentOutOfRangeException (wOfErrorMsg);
				default:
					throw new ArgumentOutOfRangeException ("Unhandled structure type passed to defineSqlQueriesToAddDealToDatabase() method of CentralCommand.");
			}
		}
		#endregion

		#region executeSqlQueriesToAddDealToDatabase
		protected void executeSqlQueriesToAddDealToDatabase (IEnumerable<string> sqlQueries)
		{
			DatabaseUpdater.ExecuteSqlQuery (sqlQueries, DatabaseConnectionString);
		}
		#endregion

		public string DatabaseConnectionString { get; set; }
	}
}
