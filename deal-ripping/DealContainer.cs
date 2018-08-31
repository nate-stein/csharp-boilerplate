using System;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Containers serve merely to store data about a particular structure and should remain 
	/// isolated from the logic that dictates how the data they store is extracted and analyzed.
	/// </summary>

	#region DealContainer
	public class DealContainer
	{
		public double InitialUnderlyingLevel { get; set; }

		public double Notional { get; set; }

		public double ParAmount { get; set; }

		public string Cusip { get; set; }

		public DateTime TradeDate { get; set; }

		public DateTime FinalValuationDate { get; set; }

		public DateTime MaturityDate { get; set; }

		public EIssuer IssuerEnum { get; set; }

		public EStructure StructureEnum { get; set; }
	}
	#endregion

	#region PrincipalProtectedNoteContainer
	public class PrincipalProtectedNoteContainer : DealContainer
	{
		public double MinimumPaymentAtMaturity { get; set; }

		public double UpsideLeverage { get; set; }

		public decimal MaxReturn { get; set; }

		public bool UpsideIsCapped { get; set; }
	}
	#endregion

	#region PLUSContainer
	public class PLUSContainer : DealContainer
	{
		public double UpsideLeverage { get; set; }

		public decimal MaxReturn { get; set; }

		public bool UpsideIsCapped { get; set; }
	}
	#endregion

	#region BufferedPLUSContainer
	public class BufferedPLUSContainer : PLUSContainer
	{
		public double Buffer { get; set; }
	}
	#endregion

	#region TriggerPLUSContainer
	public class TriggerPLUSContainer : PLUSContainer
	{
		public double KnockInBarrier { get; set; }
	}
	#endregion

	#region DualDirectionalTriggerPLUSContainer
	public class DualDirectionalTriggerPLUSContainer : TriggerPLUSContainer
	{
	}
	#endregion

	#region JumpContainer
	public class JumpContainer : DealContainer
	{
		public decimal DigitalPercentage { get; set; }

		public decimal MaxReturnIfDifferentThanDigital { get; set; }

		public bool UpsideIsCapped { get; set; }

		public bool UpsideIsCappedAtDifferentAmountThanDigital { get; set; }
	}
	#endregion

	#region BufferedJumpContainer
	public class BufferedJumpContainer : JumpContainer
	{
		public double Buffer { get; set; }
	}
	#endregion

	#region TriggerJumpContainer
	public class TriggerJumpContainer : JumpContainer
	{
		public double KnockInBarrier { get; set; }
	}
	#endregion

	#region EnhancedTriggerJumpContainer
	public class EnhancedTriggerJumpContainer : TriggerJumpContainer
	{
	}
	#endregion

	#region DualDirectionalTriggerJumpContainer
	public class DualDirectionalTriggerJumpContainer : TriggerJumpContainer
	{
	}
	#endregion

	#region AutocallableContainer
	public class AutocallableContainer : DealContainer
	{
		protected DateTime [] _callObservationDates;
		protected DateTime [] _couponObservationDates;

		public double CouponBarrier { get; set; }

		public double KnockInBarrier { get; set; }

		public decimal CouponAmountPerPeriod { get; set; }

		public string CouponFrequency { get; set; }

		public DateTime [] CallObservationDates
		{
			get { return _callObservationDates; }
			set
			{
				_callObservationDates = new DateTime [] { };
				_callObservationDates = value;
			}
		}

		public DateTime [] CouponObservationDates
		{
			get { return _couponObservationDates; }
			set
			{
				_couponObservationDates = new DateTime [] { };
				_couponObservationDates = value;
			}
		}
	}
	#endregion

	#region FixedCouponAutocallableContainer
	public class FixedCouponAutocallableContainer : AutocallableContainer
	{
	}
	#endregion

	#region StepUpAutocallableContainer
	public class StepUpAutocallableContainer : AutocallableContainer
	{
	}
	#endregion

	#region StepDownAutocallableContainer
	public class StepDownAutocallableContainer : AutocallableContainer
	{
	}
	#endregion
}
