using System;
using NUnit.Framework;
using Trinity.Utilities;
using System.Collections.Generic;
using System.Diagnostics;

namespace Trinity.DealRipping
{
	#region TestAutocallableCallObservationDatesRipper
	[TestFixture]
	public class TestAutocallableCallObservationDatesRipper
	{
		private AutocallableRipper _autocallableRipper;
		private Dictionary<string, DateTime []> _testFilePathAndExpectedCallDates;

		#region AutocallableCallDatesRipperWorks
		[Test]
		public void AutocallableCallDatesRipperWorks ()
		{
			foreach (var testPair in _testFilePathAndExpectedCallDates)
			{
				string testFilePath = testPair.Key;
				string textToParse = GeneralUtils.GetStringFromTextFile (testFilePath);
				_autocallableRipper = DealRipperFactory.GetAutocallableRipper (textToParse);
				DateTime [] expectedCallObservationDates = testPair.Value;
				DateTime [] rippedCallObservationDates = _autocallableRipper.GetCallObservationDates ();

				int expectedDateCount = expectedCallObservationDates.Length;
				int rippedDateCount = rippedCallObservationDates.Length;
				string msgOnFailure = string.Format ("The expected number of call observations ({0}) " +
																"doesn't match the ripped number of call " +
																"observations ({1}) in following file path: " +
																"{2}.",
																expectedDateCount, rippedDateCount, testFilePath);

				Assert.AreEqual (expectedDateCount, rippedDateCount, msgOnFailure);

				for (int i = 0; i < expectedCallObservationDates.Length; i++)
				{
					DateTime expectedDate = expectedCallObservationDates [i];
					DateTime rippedDate = rippedCallObservationDates [i];
					msgOnFailure = string.Format ("The call observation date we expected ({0}) " +
														  "is different than the call observation date we " +
														  "ripped ({1}) in the following file path: {2}.",
						expectedDate, rippedDate, testFilePath);
					Assert.AreEqual (expectedDate, rippedDate, msgOnFailure);
				}
			}
		}
		#endregion

		#region setupTestVariables
		[SetUp]
		protected void setupTestVariables ()
		{
			IDateArrayTestDataSource callObservationDatesSource =
				DataSourceFactory.GetCallObservationDatesFromAccessDatabase ();
			_testFilePathAndExpectedCallDates =
				callObservationDatesSource.GetTestFileAndDateArrayPairDictionary ();
		}
		#endregion
	}
	#endregion

	#region TestAutocallableCouponPerPeriodRipper
	[TestFixture]
	public class TestAutocallableCouponPerPeriodRipper
	{
		private Dictionary<string, double> _testFilePathAndExpectedCouponPerPeriod;

		#region AutocallableCouponPerPeriodRipperWorks
		[Test]
		public void AutocallableCouponPerPeriodRipperWorks ()
		{
			foreach (var testPair in _testFilePathAndExpectedCouponPerPeriod)
			{
				string testFilePath = testPair.Key;
				string textToParse = GeneralUtils.GetStringFromTextFile (testFilePath);
				AutocallableRipper ripper = DealRipperFactory.GetAutocallableRipper (textToParse);
				double expectedCoupon = testPair.Value;
				double rippedCoupon = (double)ripper.GetCouponPerPeriod ();
				string msgOnFailure = string.Format ("The coupon per period we expected ({0} doesn't " +
																"match the ripped coupon per period ({1}) for " +
																"the following file path: {2}.",
																expectedCoupon, rippedCoupon, testFilePath);
				Assert.AreEqual(expectedCoupon, rippedCoupon, msgOnFailure);
			}
		}
		#endregion

		#region setupTestVariables
		[SetUp]
		protected void setupTestVariables ()
		{
			INumberTestDataSource dataSource =
				DataSourceFactory.GetAutocallableCouponPerPeriodTestData ();
			_testFilePathAndExpectedCouponPerPeriod =
				dataSource.GetTestFileAndNumberPairDictionary ();
		}
		#endregion
	}
	#endregion

	#region TestAutocallableKnockInBarrierRipper
	[TestFixture]
	public class TestAutocallableKnockInBarrierRipper
	{
		private Dictionary<string, double> _testFilePathAndKnockInBarrierDictionary;

		#region AutocallableKnockInBarrierRipperWorks
		[Test]
		public void AutocallableKnockInBarrierRipperWorks ()
		{
			foreach (var testPair in _testFilePathAndKnockInBarrierDictionary)
			{
				string testFilePath = testPair.Key;
				string textToParse = GeneralUtils.GetStringFromTextFile (testFilePath);
				AutocallableRipper ripper = DealRipperFactory.GetAutocallableRipper (textToParse);
				double expectedKnockInBarrier = testPair.Value;
				double rippedKnockInBarrier = ripper.GetKnockInBarrier();
				Assert.AreEqual (expectedKnockInBarrier, rippedKnockInBarrier);
			}
		}
		#endregion

		#region setupTestVariables
		[SetUp]
		protected void setupTestVariables ()
		{
			INumberTestDataSource dataSource = 
				DataSourceFactory.GetAutocallableCouponPerPeriodTestData ();
			_testFilePathAndKnockInBarrierDictionary = 
				dataSource.GetTestFileAndNumberPairDictionary ();
		}
		#endregion
	}
	#endregion

	#region TestPLUSUpsideLeverageRipper
	[TestFixture]
	public class TestPLUSUpsideLeverageRipper
	{
		private PLUSRipper _plusRipper;
		private Dictionary<string, double> _testFilePathAndUpsideLeverageDictionary;

		#region PLUSUpsideLeverageRipperWorks
		[Test]
		public void PLUSUpsideLeverageRipperWorks ()
		{
			foreach (var fileNumberPair in _testFilePathAndUpsideLeverageDictionary)
			{
				string testFilePath = fileNumberPair.Key;
				Debug.WriteLine ("TestPLUSUpsideLeverageRipper testing file: " + testFilePath);
				string textToParse = GeneralUtils.GetStringFromTextFile (testFilePath);
				_plusRipper = DealRipperFactory.GetPLUSRipper (textToParse);
				double trueUpsideLeverage = fileNumberPair.Value;
				double rippedUpsideLeverage = _plusRipper.GetUpsideLeverage ();
				Assert.AreEqual (trueUpsideLeverage, rippedUpsideLeverage);
			}
		}
		#endregion

		#region setupTestVariables
		[SetUp]
		protected void setupTestVariables ()
		{
			INumberTestDataSource dataSource = DataSourceFactory.GetPLUSUpsideLeverageTestData ();
			_testFilePathAndUpsideLeverageDictionary = 
				dataSource.GetTestFileAndNumberPairDictionary ();
		}
		#endregion
	}
	#endregion

	#region TestPLUSMaxReturnRipper
	[TestFixture]
	public class TestPLUSMaxReturnRipper
	{
		private Dictionary<string, double> _expectedUpsideLeverageData;

		#region PLUSMaxReturnRipperWorks
		[Test]
		public void PLUSMaxReturnRipperWorks ()
		{
			foreach (var testPair in _expectedUpsideLeverageData)
			{
				string testFilePath = testPair.Key;
				Debug.WriteLine ("TestPLUSUpsideLeverageRipper testing file: " + testFilePath);
				string textToParse = GeneralUtils.GetStringFromTextFile (testFilePath);
				PLUSRipper ripper = DealRipperFactory.GetPLUSRipper (textToParse);
				decimal trueMaxReturn = (decimal)testPair.Value;
				decimal rippedMaxReturn = ripper.GetMaxReturn ();
				Assert.AreEqual (trueMaxReturn, rippedMaxReturn);
			}
		}
		#endregion

		#region setupTestVariables
		[SetUp]
		protected void setupTestVariables ()
		{
			INumberTestDataSource dataSource = DataSourceFactory.GetPLUSMaxReturnTestData ();
			_expectedUpsideLeverageData = dataSource.GetTestFileAndNumberPairDictionary ();
		}
		#endregion
	}
	#endregion
}
