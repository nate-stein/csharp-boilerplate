using System;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	public static class DealRipperFactory
	{
		#region GetPrincipalProtectedNoteRipper
		public static PrincipalProtectedNoteRipper GetPrincipalProtectedNoteRipper (
			string textToParse)
		{
			PrincipalProtectedNoteRipper ripper = new PrincipalProtectedNoteRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addPrincipalProtectedNoteDetailRippersToDealRipper (ripper);
			return ripper;
		}
		#endregion

		#region GetPLUSRipper
		public static PLUSRipper GetPLUSRipper (string textToParse)
		{
			PLUSRipper ripper = new PLUSRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addPLUSDetailRippersToDealRipper (ripper);
			return ripper;
		}
		#endregion

		#region GetBufferedPLUSRipper
		public static BufferedPLUSRipper GetBufferedPLUSRipper (string textToParse)
		{
			BufferedPLUSRipper buffPLUSRipper = new BufferedPLUSRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (buffPLUSRipper);
			addPLUSDetailRippersToDealRipper (buffPLUSRipper);
			buffPLUSRipper.BufferRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.Buffer);
			return buffPLUSRipper;
		}
		#endregion

		#region GetTriggerPLUSRipper
		public static TriggerPLUSRipper GetTriggerPLUSRipper (string textToParse)
		{
			TriggerPLUSRipper ripper = new TriggerPLUSRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addPLUSDetailRippersToDealRipper (ripper);
			ripper.KnockInBarrierRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.KnockInBarrier);
			return ripper;
		}
		#endregion

		#region GetDualDirectionalTriggerPLUSRipper
		public static DualDirectionalTriggerPLUSRipper GetDualDirectionalTriggerPLUSRipper (
			string textToParse)
		{
			DualDirectionalTriggerPLUSRipper dualTrigPlusRipper = (DualDirectionalTriggerPLUSRipper)GetTriggerPLUSRipper (textToParse);
			return dualTrigPlusRipper;
		}
		#endregion

		#region addBasicDetailRippersToDealRipper
		private static void addBasicDetailRippersToDealRipper (DealRipper basicRipper)
		{
			try
			{
				basicRipper.IssuerRipper = DetailRipperFactory.GetStringRipper (EDetailRipperType.Issuer);
				basicRipper.CusipRipper = DetailRipperFactory.GetStringRipper (EDetailRipperType.Cusip);
				basicRipper.FinalValuationDateRipper = DetailRipperFactory.GetDateRipper (EDetailRipperType.ValuationDate);
				basicRipper.InitialLevelRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.InitialUnderlyingLevel);
				basicRipper.ISINRipper = DetailRipperFactory.GetStringRipper (EDetailRipperType.ISIN);
				basicRipper.NotionalRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.Notional);
				basicRipper.MaturityDateRipper = DetailRipperFactory.GetDateRipper (EDetailRipperType.MaturityDate);
				basicRipper.ParRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.ParAmount);
				basicRipper.TradeDateRipper = DetailRipperFactory.GetDateRipper (EDetailRipperType.TradeDate);
				basicRipper.StructureTypeRipper = DetailRipperFactory.GetStructureTypeRipper ();
			}
			catch (Exception e)
			{
				throw new DealRipperFactoryException (
					"addBasicDetailRippersToDealRipper() encountered exception in DealRipperFactory class.", e);
			}
		}
		#endregion

		#region addPLUSDetailRippersToDealRipper
		private static void addPLUSDetailRippersToDealRipper (PLUSRipper plusRipper)
		{
			try
			{
				plusRipper.MaxReturnRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.MaxPayment);
				plusRipper.UpsideIsCappedDeterminationRipper = DetailRipperFactory.GetMatchWasFoundRipper (EDetailRipperType.UpsideIsCapped);
				plusRipper.UpsideLeverageRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.UpsideLeverage);
			}
			catch (Exception e)
			{
				throw new DealRipperFactoryException (
					"addPLUSDetailRippersToDealRipper() encountered exception in DealRipperFactory class.", e);
			}
		}
		#endregion

		#region addPrincipalProtectedNoteDetailRippersToDealRipper
		private static void addPrincipalProtectedNoteDetailRippersToDealRipper (PrincipalProtectedNoteRipper ripper)
		{
			try
			{
				ripper.MaxReturnRipper = DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.MaxPayment);
				ripper.UpsideIsCappedDeterminationRipper = DetailRipperFactory.GetMatchWasFoundRipper (
					EDetailRipperType.UpsideIsCapped);
				ripper.UpsideLeverageRipper = DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.UpsideLeverage);
			}
			catch (Exception e)
			{
				string errorMsg = "addPrincipalProtectedNoteDetailRippersToDealRipper() encountered " +
										"exception in DealRipperFactory class.";
				throw new DealRipperFactoryException (errorMsg, e);
			}
		}
		#endregion

		#region GetStandardJumpRipper
		public static JumpRipper GetStandardJumpRipper (string textToParse)
		{
			JumpRipper ripper = new JumpRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addJumpDetailRippersToDealRipper (ripper);
			return ripper;
		}
		#endregion

		#region GetBufferedJumpRipper
		public static BufferedJumpRipper GetBufferedJumpRipper (string textToParse)
		{
			BufferedJumpRipper ripper = new BufferedJumpRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addJumpDetailRippersToDealRipper (ripper);
			ripper.BufferRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.Buffer);
			return ripper;
		}
		#endregion

		#region GetTriggerJumpRipper
		public static TriggerJumpRipper GetTriggerJumpRipper (string textToParse)
		{
			TriggerJumpRipper ripper = new TriggerJumpRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addJumpDetailRippersToDealRipper (ripper);
			ripper.KnockInBarrierRipper = DetailRipperFactory.GetNumberRipper (EDetailRipperType.KnockInBarrier);
			return ripper;
		}
		#endregion

		#region GetDualDirectionalTriggerJumpRipper
		public static DualDirectionalTriggerJumpRipper GetDualDirectionalTriggerJumpRipper (string textToParse)
		{
			DualDirectionalTriggerJumpRipper dualTrigJumpRipper =
				(DualDirectionalTriggerJumpRipper)GetTriggerJumpRipper (textToParse);
			return dualTrigJumpRipper;
		}
		#endregion

		#region addJumpDetailRippersToDealRipper
		private static void addJumpDetailRippersToDealRipper (JumpRipper jumpRipper)
		{
			try
			{
				jumpRipper.DigitalRipper =
					(PriceOrPercentageRipper)DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.DigitalAmount);
				jumpRipper.UpsideIsCappedDeterminationRipper =
					DetailRipperFactory.GetMatchWasFoundRipper (EDetailRipperType.UpsideIsCapped);
			}
			catch (Exception e)
			{
				throw new DealRipperFactoryException ("addJumpDetailRippersToDealRipper() encountered " +
																  "exception in DealRipperFactory class.", e);
			}

		}
		#endregion

		#region GetAutocallableRipper
		public static AutocallableRipper GetAutocallableRipper (string textToParse)
		{
			AutocallableRipper ripper = new AutocallableRipper { TextToParse = textToParse };
			addBasicDetailRippersToDealRipper (ripper);
			addAutocallableDetailRippersToDealRipper (ripper);
			return ripper;
		}
		#endregion

		#region addAutocallableDetailRippersToDealRipper
		private static void addAutocallableDetailRippersToDealRipper (AutocallableRipper ripper)
		{
			try
			{
				ripper.CouponBarrierRipper = DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.CouponBarrier);
				ripper.KnockInBarrierRipper = DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.KnockInBarrier);
				ripper.CouponAmountRipper = (CouponRipper)DetailRipperFactory.GetNumberRipper (
					EDetailRipperType.CouponAmount);
				ripper.CouponFrequencyRipper = DetailRipperFactory.GetStringRipper (
					EDetailRipperType.CouponFrequency);
				ripper.CallObservationDatesRipper = DetailRipperFactory.GetDateArrayRipper (
					EDetailRipperType.CallObservationDates);
				ripper.CouponObservationDatesRipper = DetailRipperFactory.GetDateArrayRipper (
					EDetailRipperType.CouponObservationDates);
			}
			catch (Exception e)
			{
				throw new DealRipperFactoryException ("addAutocallableDetailRippersToDealRipper() " +
																  "encountered exception in DealRipperFactory class.",
																  e);
			}
		}
		#endregion

		#region GetFixedCouponAutocallableRipper
		public static FixedCouponAutocallableRipper GetFixedCouponAutocallableRipper (
			string textToParse)
		{
			FixedCouponAutocallableRipper ripper = new FixedCouponAutocallableRipper
			{
				TextToParse = textToParse
			};
			addBasicDetailRippersToDealRipper (ripper);
			addAutocallableDetailRippersToDealRipper (ripper);
			return ripper;
		}
		#endregion
	}
}
