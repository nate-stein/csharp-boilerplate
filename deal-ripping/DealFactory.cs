using System;
using System.Diagnostics;
using System.Linq;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// The container "factories" take a Ripper (to extract data) and use that Ripper to "fill" a 
	/// container with the relevant data.

	#region DealFactory
	public class DealFactory
	{
		#region GetDealContainer
		public virtual DealContainer GetDealContainer (DealRipper ripper)
		{
			DealContainer container = new DealContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (DealContainer container, DealRipper ripper)
		{
			try
			{
				try
				{
					string isin = ripper.GetISIN ();
					container.Cusip = isin.ExtractCusip ();
				}
				catch
				{
					container.Cusip = ripper.GetCusip ();
				}
				container.IssuerEnum = container.Cusip.DetermineIssuer ();
				container.InitialUnderlyingLevel = ripper.GetInitialLevel ();
				container.MaturityDate = ripper.GetMaturityDate ();
				container.Notional = ripper.GetNotional ();
				container.ParAmount = ripper.GetPar ();
				container.TradeDate = ripper.GetTradeDate ();
				container.StructureEnum = ripper.GetStructureEnum ();
				try
				{
					container.FinalValuationDate = ripper.GetFinalValuationDate ();
				}
				catch (DetailParsingException)
				{
					// Swallow error because several methods exist after this stage that may attempt to 
					// extract the final valuation date through other means.
				}
			}
			catch (DetailParsingException)
			{
				throw;
			}
			catch (Exception e)
			{
				string errorMsg = "Unrecognized exception encountered in DealFactory parse() method.";
				Debug.WriteLine (errorMsg);
				throw new DealFactoryException (errorMsg, e);
			}
		}
		#endregion
	}
	#endregion

	#region PrincipalProtectedNoteFactory
	public class PrincipalProtectedNoteFactory : DealFactory
	{
		#region GetDealContainer
		public virtual PrincipalProtectedNoteContainer GetDealContainer (PrincipalProtectedNoteRipper ripper)
		{
			PrincipalProtectedNoteContainer container = new PrincipalProtectedNoteContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (PrincipalProtectedNoteContainer container, PrincipalProtectedNoteRipper ripper)
		{
			base.parse (container, ripper);
			try
			{
				container.UpsideIsCapped = ripper.GetWhetherUpsideIsCapped ();
				if (container.UpsideIsCapped)
				{
					container.MaxReturn = ripper.GetMaxReturn ();
				}
				container.UpsideLeverage = ripper.GetUpsideLeverage ();
			}
			catch (DetailParsingException)
			{
				throw;
			}
			catch (Exception e)
			{
				string errorMsg = "Unrecognized exception encountered in PrincipalProtectedNoteFactory parse() method.";
				Debug.WriteLine (errorMsg);
				throw new DealFactoryException (errorMsg, e);
			}
		}
		#endregion
	}
	#endregion

	#region PLUSFactory
	public class PLUSFactory : DealFactory
	{
		#region GetDealContainer
		public virtual PLUSContainer GetDealContainer (PLUSRipper ripper)
		{
			PLUSContainer container = new PLUSContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (PLUSContainer container, PLUSRipper ripper)
		{
			base.parse (container, ripper);
			try
			{
				container.UpsideIsCapped = ripper.GetWhetherUpsideIsCapped ();
				if (container.UpsideIsCapped)
				{
					container.MaxReturn = ripper.GetMaxReturn ();
				}
				container.UpsideLeverage = ripper.GetUpsideLeverage ();
			}
			catch (DetailParsingException)
			{
				throw;
			}
			catch (Exception e)
			{
				string errorMsg = "Unrecognized exception encountered in PLUSFactory parse() method.";
				Debug.WriteLine (errorMsg);
				throw new DealFactoryException (errorMsg, e);
			}
		}
		#endregion
	}
	#endregion

	#region BufferedPLUSFactory
	public class BufferedPLUSFactory : PLUSFactory
	{
		#region GetDealContainer
		public virtual BufferedPLUSContainer GetDealContainer (BufferedPLUSRipper ripper)
		{
			BufferedPLUSContainer container = new BufferedPLUSContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (BufferedPLUSContainer container, BufferedPLUSRipper ripper)
		{
			base.parse (container, ripper);
			container.Buffer = ripper.GetBuffer ();
		}
		#endregion
	}
	#endregion

	#region TriggerPLUSFactory
	public class TriggerPLUSFactory : PLUSFactory
	{
		#region GetDealContainer
		public virtual TriggerPLUSContainer GetDealContainer (TriggerPLUSRipper ripper)
		{
			TriggerPLUSContainer container = new TriggerPLUSContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (TriggerPLUSContainer container, TriggerPLUSRipper ripper)
		{
			base.parse (container, ripper);
			double knockInBarrier = ripper.GetKnockInBarrier ();
			// If the KI Barrier is less than one, it is likely that it represents a % amount.
			if (knockInBarrier < 1)
			{
				knockInBarrier = knockInBarrier * container.InitialUnderlyingLevel;
			}
			container.KnockInBarrier = knockInBarrier;
		}
		#endregion
	}
	#endregion

	#region DualDirectionalTriggerPLUSFactory
	/// <summary>
	/// This container factory is exactly the same as the TriggerPLUSFactory so we don't extend it at all.
	/// </summary>
	public class DualDirectionalTriggerPLUSFactory : TriggerPLUSFactory
	{
		#region GetDealContainer
		public virtual DualDirectionalTriggerPLUSContainer GetDealContainer (DualDirectionalTriggerPLUSRipper ripper)
		{
			DualDirectionalTriggerPLUSContainer container = new DualDirectionalTriggerPLUSContainer ();
			base.parse (container, ripper);
			return container;
		}
		#endregion
	}
	#endregion

	#region JumpFactory
	public class JumpFactory : DealFactory
	{
		#region GetDealContainer
		public virtual JumpContainer GetDealContainer (JumpRipper ripper)
		{
			JumpContainer container = new JumpContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (JumpContainer container, JumpRipper ripper)
		{
			base.parse (container, ripper);
			try
			{
				container.DigitalPercentage = ripper.GetDigitalPercentage ();
				container.UpsideIsCapped = ripper.GetWhetherUpsideIsCapped ();
				if (container.UpsideIsCapped)
				{
					container.UpsideIsCappedAtDifferentAmountThanDigital = false;
				}
			}
			catch (DetailParsingException)
			{
				throw;
			}
			catch (Exception e)
			{
				string errorMsg = "Unrecognized exception encountered in JumpFactory parse() method.";
				Debug.WriteLine (errorMsg);
				throw new DealFactoryException (errorMsg, e);
			}
		}
		#endregion
	}
	#endregion

	#region BufferedJumpFactory
	public class BufferedJumpFactory : JumpFactory
	{
		#region GetDealContainer
		public virtual BufferedJumpContainer GetDealContainer (BufferedJumpRipper ripper)
		{
			BufferedJumpContainer container = new BufferedJumpContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (BufferedJumpContainer container, BufferedJumpRipper ripper)
		{
			base.parse (container, ripper);
			container.Buffer = ripper.GetBuffer ();
		}
		#endregion
	}
	#endregion

	#region TriggerJumpFactory
	public class TriggerJumpFactory : JumpFactory
	{
		#region GetDealContainer
		public virtual TriggerJumpContainer GetDealContainer (TriggerJumpRipper ripper)
		{
			TriggerJumpContainer container = new TriggerJumpContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (TriggerJumpContainer container, TriggerJumpRipper ripper)
		{
			base.parse (container, ripper);
			double knockInBarrier = ripper.GetKnockInBarrier ();
			// If the KI Barrier is less than one, it is likely that it represents a % amount.
			if (knockInBarrier < 1)
			{
				knockInBarrier = knockInBarrier * container.InitialUnderlyingLevel;
			}
			container.KnockInBarrier = knockInBarrier;
		}
		#endregion
	}
	#endregion

	#region DualDirectionalTriggerJumpFactory
	/// <summary>
	/// This container factory is exactly the same as the TriggerJumpFactory so we don't extend it at all.
	/// </summary>
	public class DualDirectionalTriggerJumpFactory : TriggerJumpFactory
	{
		#region GetDealContainer
		public virtual DualDirectionalTriggerJumpContainer GetDealContainer (DualDirectionalTriggerJumpRipper ripper)
		{
			DualDirectionalTriggerJumpContainer container = new DualDirectionalTriggerJumpContainer ();
			base.parse (container, ripper);
			return container;
		}
		#endregion
	}
	#endregion

	#region EnhancedTriggerJumpFactory
	/// <summary>
	/// This container factory is exactly the same as the TriggerJumpFactory so we don't extend it at all.
	/// We keep dependence on a TriggerJumpRipper because the Enhanced version requires no additional parsing.
	/// </summary>
	public class EnhancedTriggerJumpFactory : TriggerJumpFactory
	{
		#region GetDealContainer
		public virtual new EnhancedTriggerJumpContainer GetDealContainer (TriggerJumpRipper ripper)
		{
			EnhancedTriggerJumpContainer container = new EnhancedTriggerJumpContainer ();
			base.parse (container, ripper);
			return container;
		}
		#endregion
	}
	#endregion

	#region AutocallableFactory
	public class AutocallableFactory : DealFactory
	{
		#region GetDealContainer
		public virtual AutocallableContainer GetDealContainer (AutocallableRipper ripper)
		{
			AutocallableContainer container = new AutocallableContainer ();
			parse (container, ripper);
			return container;
		}
		#endregion

		#region parse
		protected virtual void parse (AutocallableContainer container, AutocallableRipper ripper)
		{
			base.parse (container, ripper);
			DateTime [] couponObservationDates;
			try
			{
				DateTime [] callObservationDates = ripper.GetCallObservationDates ();
				container.CallObservationDates = callObservationDates;
				try
				{
					couponObservationDates = ripper.GetCouponObservationDates ();
					container.CouponObservationDates = couponObservationDates;
				}
				catch
				{
					string warning = "Unable to extract coupon observation dates on their own so " +
										  "assuming they are the same as the call observation dates.";
					Debug.WriteLine (warning);
					container.CouponObservationDates = callObservationDates;
				}
			}
			catch (DetailParsingException)
			{
				string warning = "Unable to extract call observation dates on their own so assuming " +
									  "they are the same as the coupon observation dates.";
				Debug.WriteLine (warning);
				couponObservationDates = ripper.GetCouponObservationDates ();
				container.CouponObservationDates = couponObservationDates;
				container.CallObservationDates = couponObservationDates;
			}
			container.CouponAmountPerPeriod = ripper.GetCouponPerPeriod ();
			container.CouponBarrier = ripper.GetCouponBarrier ();
			double knockInBarrier = ripper.GetKnockInBarrier ();
			// If the KI Barrier is less than one, it is likely that it represents a % amount.
			if (knockInBarrier < 1)
			{
				knockInBarrier = knockInBarrier * container.InitialUnderlyingLevel;
			}
			container.KnockInBarrier = knockInBarrier;
			// If we fail to rip the coupon frequency, we "swallow" the error because we can 
			// ultimately determine it by the distance between coupon observation dates.
			try
			{
				if (ripper.CouponFrequency == null)
				{
					container.CouponFrequency = ripper.GetCouponFrequency ();
				}
				else container.CouponFrequency = ripper.CouponFrequency;
			}
			catch (Exception) { }

			// If we failed to parse the Final Valuation Date on its own, then we set it equal to the last coupon
			// observation date
			if (container.FinalValuationDate.IsEmpty ())
			{
				string warning = String.Format ("Final Valuation Date empty in AutocallableFactory for " +
														  "Cusip {0}. Therefore, assuming it is the final coupon " +
														  "observation date.", container.Cusip);
				Debug.WriteLine (warning);
				container.FinalValuationDate = container.CouponObservationDates.Last ();
			}
		}
		#endregion
	}
	#endregion

	#region StepUpAutocallableFactory
	public class StepUpAutocallableFactory : AutocallableFactory
	{
		public new virtual StepUpAutocallableContainer GetDealContainer (AutocallableRipper ripper)
		{
			StepUpAutocallableContainer container = new StepUpAutocallableContainer ();
			parse (container, ripper);
			return container;
		}
	}
	#endregion

	#region StepDownAutocallableFactory
	public class StepDownAutocallableFactory : AutocallableFactory
	{
		public new virtual StepDownAutocallableContainer GetDealContainer (AutocallableRipper ripper)
		{
			StepDownAutocallableContainer container = new StepDownAutocallableContainer ();
			parse (container, ripper);
			return container;
		}
	}
	#endregion
}
