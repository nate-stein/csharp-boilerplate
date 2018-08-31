using System;
using System.Diagnostics;
using System.Collections.Generic;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// The SqlQueryGenerator classes use the details stored in "Container" objects in order to 
	/// produce the actual executable SQL queries that will update the database with new records. 
	/// Consequently, they have to have a good degree of knowledge about the Containers.

	#region ISqlQueryGenerator
	/// <summary>
	/// Principal responsibility is to create the sql queries that are necessary to add a 
	/// structured product to the database. Any given Container object passed to the 
	/// SqlQueryFactory may have data that is output to multiple tables in the database, so the 
	/// GetSqlQueriesForAddingDealToDatabase() method returns a List of strings.
	/// </summary>
	public interface ISqlQueryGenerator
	{
		string [] GetSqlQueriesForAddingDealToDatabase ();
	}
	#endregion

	#region GeneralSqlQueryGenerator
	public abstract class GeneralSqlQueryGenerator : ISqlQueryGenerator
	{
		protected DealContainer _dealContainer;
		protected List<string> _sqlQueriesList;
		protected Dictionary<string, string> _structureSpecificFieldNamesAndValues;

		protected GeneralSqlQueryGenerator ()
		{
			_sqlQueriesList = new List<string> ();
			_structureSpecificFieldNamesAndValues = new Dictionary<string, string> ();
		}

		public abstract string [] GetSqlQueriesForAddingDealToDatabase ();

		#region addSqlQueryForStructureSpecificDetails
		protected void addSqlQueryForStructureSpecificDetails (string databaseTableName)
		{
			string sqlQuery =
				"INSERT INTO " + databaseTableName + " " +
				 "(" + createStructureSpecificTableFieldNamesSql () + ") " +
				 "VALUES (" + createStructureSpecificTableFieldValuesSql () + ")";
			_sqlQueriesList.Add (sqlQuery);
		}
		#endregion

		#region createStructureSpecificTableFieldNamesSql
		// Creates portion of SQL query that will contain the field names for the structure-specific 
		// details.
		protected virtual string createStructureSpecificTableFieldNamesSql ()
		{
			string fieldNamesPortionOfSqlQuery = "";

			foreach (var entry in _structureSpecificFieldNamesAndValues)
			{
				fieldNamesPortionOfSqlQuery = fieldNamesPortionOfSqlQuery + entry.Key + ", ";
			}
			// Remove the last ", "
			fieldNamesPortionOfSqlQuery =
				fieldNamesPortionOfSqlQuery.Substring (0, fieldNamesPortionOfSqlQuery.Length - 2);
			return fieldNamesPortionOfSqlQuery;
		}
		#endregion

		#region createStructureSpecificTableFieldValuesSql
		// Creates portion of SQL query that will contain the field values for the 
		// structure-specific details.
		protected virtual string createStructureSpecificTableFieldValuesSql ()
		{
			string fieldValuesPortionOfSqlQuery = "";

			foreach (var entry in _structureSpecificFieldNamesAndValues)
			{
				fieldValuesPortionOfSqlQuery = fieldValuesPortionOfSqlQuery + entry.Value + ", ";
			}
			// Remove the last ", "
			fieldValuesPortionOfSqlQuery =
				fieldValuesPortionOfSqlQuery.Substring (0, fieldValuesPortionOfSqlQuery.Length - 2);
			return fieldValuesPortionOfSqlQuery;
		}
		#endregion

		#region addSqlQueryForGeneralSPDetails
		protected virtual void addSqlQueryForGeneralSPDetails ()
		{
			string sqlQuery = createSqlQueryForGeneralSPDetails (
				_dealContainer.Cusip,
				_dealContainer.TradeDate,
				_dealContainer.FinalValuationDate,
				_dealContainer.MaturityDate,
				_dealContainer.Notional,
				_dealContainer.InitialUnderlyingLevel,
				_dealContainer.ParAmount,
				_dealContainer.IssuerEnum,
				_dealContainer.StructureEnum);

			_sqlQueriesList.Add (sqlQuery);
		}
		#endregion

		#region createSqlQueryForGeneralSPDetails
		protected virtual string createSqlQueryForGeneralSPDetails (
			string cusip,
			DateTime tradeDate,
			DateTime finalValuationDate,
			DateTime maturityDate,
			double notional,
			double initialLevel,
			double parAmount,
			EIssuer issuer,
			EStructure structure)
		{
			string tradeDateText = tradeDate.ToDatabaseText ();
			string valuationDateText = finalValuationDate.ToDatabaseText ();
			string maturityDateText = maturityDate.ToDatabaseText ();
			string issuerText = issuer.ToStringShortCode ();
			string structureDescription = structure.ToDescription ();

			string insertPortion =
				"INSERT INTO " + SPDatabaseUtils.TableNameGeneralDetails + " " +
				"(Cusip, Trade_Date, Final_Valuation_Date, Maturity_Date, Notional, Initial_Level, " +
				"Par_Amount, Issuer, Structure)";
			string valuesPortion = string.Format (@"VALUES ('{0}', #{1}#, #{2}#, #{3}#, {4}, {5}, {6}, '{7}', '{8}')",
				 cusip, tradeDateText, valuationDateText, maturityDateText, notional, initialLevel, parAmount, issuerText, structureDescription);

			return insertPortion + " " + valuesPortion;
		}
		#endregion

		protected virtual DealContainer Container
		{
			set { _dealContainer = value; }
		}
	}
	#endregion

	#region PrincipalProtectedNoteSqlQueryGenerator
	public class PrincipalProtectedNoteSqlQueryGenerator : GeneralSqlQueryGenerator
	{
		protected PrincipalProtectedNoteContainer _ppnContainer;

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues (_ppnContainer);
			addSqlQueryForStructureSpecificDetails (SPDatabaseUtils.TableNamePrincipalProtectedNote);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues (
			PrincipalProtectedNoteContainer container)
		{
			_structureSpecificFieldNamesAndValues.Add ("Cusip", "'" + container.Cusip + "'");
			_structureSpecificFieldNamesAndValues.Add (
				"Upside_Leverage", container.UpsideLeverage.ToString ());
			if (_ppnContainer.UpsideIsCapped)
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "True");
				_structureSpecificFieldNamesAndValues.Add ("Max_Return", container.MaxReturn.ToString ());
			}
			else
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "False");
			}
		}
		#endregion

		#region Properties
		public new virtual PrincipalProtectedNoteContainer Container
		{
			set
			{
				_ppnContainer = value;
				base.Container = _ppnContainer;
			}
		}
		#endregion
	}
	#endregion

	#region PlusSqlQueryGenerator
	public class PlusSqlQueryGenerator : GeneralSqlQueryGenerator
	{
		protected PLUSContainer _plusContainer;
		protected const string _TBLNAME_PLUS = "PLUS";

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues (_plusContainer);
			addSqlQueryForStructureSpecificDetails (_TBLNAME_PLUS);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues (PLUSContainer container)
		{
			string subtype = container.StructureEnum.DatabaseSubtype ();
			_structureSpecificFieldNamesAndValues.Add ("Subtype", "'" + subtype + "'");
			_structureSpecificFieldNamesAndValues.Add ("Cusip", "'" + container.Cusip + "'");
			_structureSpecificFieldNamesAndValues.Add ("Upside_Leverage", container.UpsideLeverage.ToString ());
			if (_plusContainer.UpsideIsCapped)
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "True");
				_structureSpecificFieldNamesAndValues.Add ("Max_Return", container.MaxReturn.ToString ());
			}
			else
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "False");
			}
		}
		#endregion

		#region Properties
		public new virtual PLUSContainer Container
		{
			set
			{
				_plusContainer = value;
				base.Container = _plusContainer;
			}
		}
		#endregion
	}
	#endregion

	#region BufferedPlusSqlQueryGenerator
	public class BufferedPLUSSqlQueryGenerator : PlusSqlQueryGenerator
	{
		protected BufferedPLUSContainer _bufferedPLUSContainer;

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues ();
			addSqlQueryForStructureSpecificDetails (_TBLNAME_PLUS);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_bufferedPLUSContainer);
			_structureSpecificFieldNamesAndValues.Add ("Buffer", _bufferedPLUSContainer.Buffer.ToString ());
		}
		#endregion

		#region Properties
		public new BufferedPLUSContainer Container
		{
			set
			{
				_bufferedPLUSContainer = value;
				base.Container = _bufferedPLUSContainer;
			}
		}
		#endregion
	}
	#endregion

	#region TriggerPlusSqlQueryGenerator
	public class TriggerPlusSqlQueryGenerator : PlusSqlQueryGenerator
	{
		protected TriggerPLUSContainer _triggerPLUSContainer;

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues ();
			addSqlQueryForStructureSpecificDetails (_TBLNAME_PLUS);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_triggerPLUSContainer);
			_structureSpecificFieldNamesAndValues.Add ("KI_Barrier", _triggerPLUSContainer.KnockInBarrier.ToString ());
		}
		#endregion

		#region Properties
		public new TriggerPLUSContainer Container
		{
			set
			{
				_triggerPLUSContainer = value;
				base.Container = _triggerPLUSContainer;
			}
		}
		#endregion
	}
	#endregion

	#region DualDirectionalTriggerPlusSqlQueryGenerator
	public class DualDirectionalTriggerPlusSqlQueryGenerator : TriggerPlusSqlQueryGenerator
	{
		protected DualDirectionalTriggerPLUSContainer _dualDirectionalTriggerPLUSContainer;

		#region updateStructureSpecificFieldNamesAndValues
		protected override void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_dualDirectionalTriggerPLUSContainer);
		}
		#endregion

		#region Properties
		public new DualDirectionalTriggerPLUSContainer Container
		{
			set
			{
				_dualDirectionalTriggerPLUSContainer = value;
				base.Container = _dualDirectionalTriggerPLUSContainer;
			}
		}
		#endregion
	}
	#endregion

	#region JumpSqlQueryGenerator
	public class JumpSqlQueryGenerator : GeneralSqlQueryGenerator
	{
		protected JumpContainer _jumpContainer;
		protected const string _TBLNAME_JUMP = "Jump";

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues (_jumpContainer);
			addSqlQueryForStructureSpecificDetails (_TBLNAME_JUMP);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues (JumpContainer container)
		{
			string subtype = container.StructureEnum.DatabaseSubtype ();
			_structureSpecificFieldNamesAndValues.Add ("Subtype", "'" + subtype + "'");
			_structureSpecificFieldNamesAndValues.Add ("Cusip", "'" + container.Cusip + "'");
			_structureSpecificFieldNamesAndValues.Add ("Digital_Percentage", container.DigitalPercentage.ToString ());
			if (_jumpContainer.UpsideIsCapped)
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "True");
				if (_jumpContainer.UpsideIsCappedAtDifferentAmountThanDigital)
				{
					_structureSpecificFieldNamesAndValues.Add ("Max_Return_If_Not_Digital", _jumpContainer.MaxReturnIfDifferentThanDigital.ToString ());
					_structureSpecificFieldNamesAndValues.Add ("Upside_Capped_At_Digital", "False");
				}
				else
				{
					_structureSpecificFieldNamesAndValues.Add ("Upside_Capped_At_Digital", "True");
				}
			}
			else
			{
				_structureSpecificFieldNamesAndValues.Add ("Upside_Is_Capped", "False");
			}
		}
		#endregion

		#region Properties
		public new virtual JumpContainer Container
		{
			set
			{
				_jumpContainer = value;
				base.Container = _jumpContainer;
			}
		}
		#endregion
	}
	#endregion

	#region BufferedJumpSqlQueryGenerator
	public class BufferedJumpSqlQueryGenerator : JumpSqlQueryGenerator
	{
		protected BufferedJumpContainer _bufferedJumpContainer;

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues ();
			addSqlQueryForStructureSpecificDetails (_TBLNAME_JUMP);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_bufferedJumpContainer);
			_structureSpecificFieldNamesAndValues.Add ("Buffer", _bufferedJumpContainer.Buffer.ToString ());
		}
		#endregion

		#region Properties
		public new BufferedJumpContainer Container
		{
			set
			{
				_bufferedJumpContainer = value;
				base.Container = _bufferedJumpContainer;
			}
		}
		#endregion
	}
	#endregion

	#region TriggerJumpSqlQueryGenerator
	public class TriggerJumpSqlQueryGenerator : JumpSqlQueryGenerator
	{
		protected TriggerJumpContainer _triggerJumpContainer;

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues ();
			addSqlQueryForStructureSpecificDetails (_TBLNAME_JUMP);
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_triggerJumpContainer);
			_structureSpecificFieldNamesAndValues.Add ("KI_Barrier", _triggerJumpContainer.KnockInBarrier.ToString ());
		}
		#endregion

		#region Properties
		public new TriggerJumpContainer Container
		{
			set
			{
				_triggerJumpContainer = value;
				base.Container = _triggerJumpContainer;
			}
		}
		#endregion
	}
	#endregion

	#region DualDirectionalTriggerJumpSqlQueryGenerator
	public class DualDirectionalTriggerJumpSqlQueryGenerator : TriggerJumpSqlQueryGenerator
	{
		protected DualDirectionalTriggerJumpContainer _dualDirectionalTriggerJumpContainer;

		#region updateStructureSpecificFieldNamesAndValues
		protected override void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_dualDirectionalTriggerJumpContainer);
		}
		#endregion

		#region Properties
		public new DualDirectionalTriggerJumpContainer Container
		{
			set
			{
				_dualDirectionalTriggerJumpContainer = value;
				base.Container = _dualDirectionalTriggerJumpContainer;
			}
		}
		#endregion
	}
	#endregion

	#region AutocallableSqlQueryGenerator
	public class AutocallableSqlQueryGenerator : GeneralSqlQueryGenerator
	{
		#region Members
		protected AutocallableContainer _autocallableContainer;
		protected Dictionary<string, string> _callObservationDateColumnsAndValues;
		protected Dictionary<string, string> _couponObservationDateColumnsAndValues;
		protected const string _TBLNAME_AUTOCALLABLE = "Autocallable";
		protected const string _TBLNAME_CALL_OBSERVATION_DATES = "CallObservationDates";
		protected const string _TBLNAME_COUPON_OBSERVATION_DATES = "CouponObservationDates";
		#endregion

		#region GetSqlQueriesForAddingDealToDatabase
		public override string [] GetSqlQueriesForAddingDealToDatabase ()
		{
			addSqlQueryForGeneralSPDetails ();
			updateStructureSpecificFieldNamesAndValues (_autocallableContainer);
			addSqlQueryForStructureSpecificDetails (_TBLNAME_AUTOCALLABLE);
			addSqlQueryForAddingCallObservationDates ();
			addSqlQueryForAddingCouponObservationDates ();
			return _sqlQueriesList.ToArray ();
		}
		#endregion

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues (AutocallableContainer container)
		{
			Debug.WriteLine ("Called updateStructureSpecificFieldNamesAndValues() in AutocallableSqlQueryGenerator class.");
			string subtype = container.StructureEnum.DatabaseSubtype ();
			_structureSpecificFieldNamesAndValues.Add ("Subtype", "'" + subtype + "'");
			_structureSpecificFieldNamesAndValues.Add ("Cusip", "'" + container.Cusip + "'");
			_structureSpecificFieldNamesAndValues.Add ("Coupon_Barrier", container.CouponBarrier.ToString ());
			_structureSpecificFieldNamesAndValues.Add ("KI_Barrier", container.KnockInBarrier.ToString ());
			_structureSpecificFieldNamesAndValues.Add ("Coupon_Per_Period", container.CouponAmountPerPeriod.ToString ());
			if (!(container.CouponFrequency == null))
			{
				_structureSpecificFieldNamesAndValues.Add ("Coupon_Frequency", "'" + container.CouponFrequency + "'");
			}
		}
		#endregion

		#region addSqlQueryForAddingCallObservationDates
		protected virtual void addSqlQueryForAddingCallObservationDates ()
		{
			updateCallObservationFieldNamesAndValues ();

			string callDatesPortionOfSqlQuery = "";
			string callFieldNamesPortionOfSqlQuery = "";

			foreach (var entry in _callObservationDateColumnsAndValues)
			{
				callFieldNamesPortionOfSqlQuery = callFieldNamesPortionOfSqlQuery + entry.Key + ", ";
				callDatesPortionOfSqlQuery = callDatesPortionOfSqlQuery + entry.Value + ", ";
			}
			// Remove the last ", "
			callFieldNamesPortionOfSqlQuery = callFieldNamesPortionOfSqlQuery.Substring (0, callFieldNamesPortionOfSqlQuery.Length - 2);
			callDatesPortionOfSqlQuery = callDatesPortionOfSqlQuery.Substring (0, callDatesPortionOfSqlQuery.Length - 2);

			string sqlQuery = "INSERT INTO " + _TBLNAME_CALL_OBSERVATION_DATES + " " +
				 "(Cusip, " + callFieldNamesPortionOfSqlQuery + ") " +
				 "VALUES ('" + _autocallableContainer.Cusip + "', " + callDatesPortionOfSqlQuery + ")";

			_sqlQueriesList.Add (sqlQuery);
		}

		#region updateCallObservationFieldNamesAndValues
		protected void updateCallObservationFieldNamesAndValues ()
		{
			Debug.WriteLine ("Called updateCallObservationFieldNamesAndValues() in AutocallableSqlQueryGenerator class.");
			_callObservationDateColumnsAndValues = new Dictionary<string, string> ();
			try
			{
				for (int q = 0; q < _autocallableContainer.CallObservationDates.Length; q++)
				{
					string callObservationDate = _autocallableContainer.CallObservationDates [q].ToString ("yyyy-MM-dd HH:mm:ss");
					_callObservationDateColumnsAndValues.Add ("Call_Date_" + (q + 1), "#" + callObservationDate + "#");
				}
			}
			catch (Exception e)
			{
				string errorMsg = "updateCallObservationFieldNamesAndValues() method of AutocallableSqlQueryGenerator class encountered an exception.";
				throw new SqlQueryCreationException (errorMsg, e);
			}
		}
		#endregion
		#endregion

		#region addSqlQueryForAddingCouponObservationDates
		protected virtual void addSqlQueryForAddingCouponObservationDates ()
		{
			updateCouponObservationFieldNamesAndValues ();

			string couponDatesPortionOfSqlQuery = "";
			string couponFieldNamesPortionOfSqlQuery = "";

			foreach (var entry in _couponObservationDateColumnsAndValues)
			{
				couponFieldNamesPortionOfSqlQuery = couponFieldNamesPortionOfSqlQuery + entry.Key + ", ";
				couponDatesPortionOfSqlQuery = couponDatesPortionOfSqlQuery + entry.Value + ", ";
			}
			// Remove the last ", "
			couponFieldNamesPortionOfSqlQuery = couponFieldNamesPortionOfSqlQuery.Substring (0, couponFieldNamesPortionOfSqlQuery.Length - 2);
			couponDatesPortionOfSqlQuery = couponDatesPortionOfSqlQuery.Substring (0, couponDatesPortionOfSqlQuery.Length - 2);

			string sqlQuery = "INSERT INTO " + _TBLNAME_COUPON_OBSERVATION_DATES + " " +
				 "(Cusip, " + couponFieldNamesPortionOfSqlQuery + ") " +
				 "VALUES ('" + _autocallableContainer.Cusip + "', " + couponDatesPortionOfSqlQuery + ")";

			_sqlQueriesList.Add (sqlQuery);
		}

		#region updateCouponObservationFieldNamesAndValues
		protected void updateCouponObservationFieldNamesAndValues ()
		{
			Debug.WriteLine ("Called updateCouponObservationFieldNamesAndValues() in AutocallableSqlQueryGenerator class.");
			_couponObservationDateColumnsAndValues = new Dictionary<string, string> ();
			try
			{
				for (int q = 0; q < _autocallableContainer.CouponObservationDates.Length; q++)
				{
					string couponObservationDate = _autocallableContainer.CouponObservationDates [q].ToString ("yyyy-MM-dd HH:mm:ss");
					_couponObservationDateColumnsAndValues.Add ("Coupon_Date_" + (q + 1), "#" + couponObservationDate + "#");
				}
			}
			catch (Exception e)
			{
				string errorMsg = "updateCouponObservationFieldNamesAndValues() method of AutocallableSqlQueryGenerator class encountered an exception.";
				throw new SqlQueryCreationException (errorMsg, e);
			}
		}
		#endregion
		#endregion

		#region Properties
		public new virtual AutocallableContainer Container
		{
			set
			{
				_autocallableContainer = value;
				base.Container = _autocallableContainer;
			}
		}
		#endregion
	}
	#endregion

	#region StepUpAutocallableSqlQueryGenerator
	public class StepUpAutocallableSqlQueryGenerator : AutocallableSqlQueryGenerator
	{
		protected StepUpAutocallableContainer _stepUpAutocallableContainer;

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_stepUpAutocallableContainer);
		}
		#endregion

		#region Properties
		public new StepUpAutocallableContainer Container
		{
			set
			{
				_stepUpAutocallableContainer = value;
				base.Container = _stepUpAutocallableContainer;
			}
		}
		#endregion
	}
	#endregion

	#region StepDownAutocallableSqlQueryGenerator
	public class StepDownAutocallableSqlQueryGenerator : AutocallableSqlQueryGenerator
	{
		protected StepDownAutocallableContainer _stepDownAutocallableContainer;

		#region updateStructureSpecificFieldNamesAndValues
		protected virtual void updateStructureSpecificFieldNamesAndValues ()
		{
			base.updateStructureSpecificFieldNamesAndValues (_stepDownAutocallableContainer);
		}
		#endregion

		#region Properties
		public new StepDownAutocallableContainer Container
		{
			set
			{
				_stepDownAutocallableContainer = value;
				base.Container = _stepDownAutocallableContainer;
			}
		}
		#endregion
	}
	#endregion
}
