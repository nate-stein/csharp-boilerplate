using System;
using System.Diagnostics;
using Trinity.SPDetailRipping;

namespace Trinity.DealRipping
{
	/// <summary>
	/// All classes derived from DealRipper are intended to serve as a container (i.e. storage 
	/// class) for the various rippers needed to extract all the relevant data for a given 
	/// Structured Product. Therefore, their methods and properties should strongly parallel those 
	/// properties of their corresponding DealContainer classes. In other words, most data fields 
	/// in a class derived from DealContainer require a corresponding extraction method in those 
	/// classes stored as a method in a DealRipper.
	/// </summary>

	#region DealRipper
	public class DealRipper
	{
		protected string _textToParse;

		#region Methods
		/// <summary>
		/// We use no error handling procedures with these methods because the only exceptions we 
		/// will encounter will be when the details being ripped are not found, in which case we 
		/// want that exception to be thrown up.
		/// </summary>
		/// <returns></returns>
		public virtual double GetInitialLevel ()
		{
			return InitialLevelRipper.Get (ref _textToParse);
		}

		public virtual double GetNotional ()
		{
			return NotionalRipper.Get (ref _textToParse);
		}

		public virtual double GetPar ()
		{
			return ParRipper.Get (ref _textToParse);
		}

		public virtual string GetIssuer ()
		{
			string issuer = IssuerRipper.Get (ref _textToParse);
			return issuer;
		}

		public virtual string GetISIN ()
		{
			return ISINRipper.Get (ref _textToParse);
		}

		public virtual string GetCusip ()
		{
			try
			{
				return CusipRipper.Get (ref _textToParse);
			}
			catch (DetailParsingException)
			{
				Debug.WriteLine ("DealRipper failed to get Cusip on its own so attempting to return " +
									  "Cusip through ISIN value.");
				return GetISIN ().ExtractCusip();
			}
		}

		public virtual DateTime GetTradeDate ()
		{
			return TradeDateRipper.Get (ref _textToParse);
		}

		public virtual DateTime GetFinalValuationDate ()
		{
			return FinalValuationDateRipper.Get (ref _textToParse);
		}

		public virtual DateTime GetMaturityDate ()
		{
			return MaturityDateRipper.Get (ref _textToParse);
		}

		public virtual EStructure GetStructureEnum ()
		{
			return StructureTypeRipper.Get (ref _textToParse);
		}
		#endregion

		#region Properties
		public string TextToParse
		{
			get { return _textToParse; }
			set { _textToParse = value; }
		}

		public IDetailRipper<double> InitialLevelRipper { get; set; }

		public IDetailRipper<double> NotionalRipper { get; set; }

		public IDetailRipper<double> ParRipper { get; set; }

		public IDetailRipper<string> IssuerRipper { get; set; }

		public IDetailRipper<string> ISINRipper { get; set; }

		public IDetailRipper<string> CusipRipper { get; set; }

		public IDetailRipper<DateTime> TradeDateRipper { get; set; }

		public IDetailRipper<DateTime> FinalValuationDateRipper { get; set; }

		public IDetailRipper<DateTime> MaturityDateRipper { get; set; }

		public IDetailRipper<EStructure> StructureTypeRipper { get; set; }

		#endregion
	}
	#endregion

	#region PrincipalProtectedNoteRipper
	public class PrincipalProtectedNoteRipper : DealRipper
	{
		public virtual double GetUpsideLeverage ()
		{
			double upsideLeverage = UpsideLeverageRipper.Get (ref _textToParse);
			return upsideLeverage;
		}

		public virtual decimal GetMaxReturn ()
		{
			MaxPaymentRipper maxPaymentRipper = (MaxPaymentRipper)MaxReturnRipper;
			decimal maxPayment = (decimal)maxPaymentRipper.Get (ref _textToParse);
			if (maxPaymentRipper.PaymentRepresentsPercentageOfPar)
			{
				if (maxPaymentRipper.MaxPaymentNumberIncludesPar) return maxPayment - 1;
				else return maxPayment;
			}
			else
			{
				decimal par = (decimal)GetPar ();
				if (maxPaymentRipper.MaxPaymentNumberIncludesPar)
				{
					decimal maxReturnExcludingPar = (maxPayment / par) - 1;
					return maxReturnExcludingPar;
				}
				else return maxPayment / par;
			}
		}

		public virtual bool GetWhetherUpsideIsCapped ()
		{
			bool isUpsideCapped = UpsideIsCappedDeterminationRipper.Get (ref _textToParse);
			return isUpsideCapped;
		}

		#region Properties
		public IDetailRipper<double> UpsideLeverageRipper { get; set; }

		public IDetailRipper<double> MaxReturnRipper { get; set; }

		public IDetailRipper<bool> UpsideIsCappedDeterminationRipper { get; set; }

		#endregion
	}
	#endregion

	#region PLUSRipper
	public class PLUSRipper : DealRipper
	{
		public virtual double GetUpsideLeverage ()
		{
			double upsideLeverage = UpsideLeverageRipper.Get (ref _textToParse);
			return upsideLeverage;
		}

		public virtual decimal GetMaxReturn ()
		{
			MaxPaymentRipper maxPaymentRipper = (MaxPaymentRipper)MaxReturnRipper;
			decimal maxPayment = (decimal)maxPaymentRipper.Get (ref _textToParse);
			if (maxPaymentRipper.PaymentRepresentsPercentageOfPar) return maxPayment;
			else
			{
				decimal par = (decimal)GetPar ();
				if (maxPaymentRipper.MaxPaymentNumberIncludesPar)
				{
					decimal returnExcludingPar = (maxPayment - par) / par;
					return returnExcludingPar;
				}
				else return maxPayment / par;
			}
		}

		public virtual bool GetWhetherUpsideIsCapped ()
		{
			bool isUpsideCapped = UpsideIsCappedDeterminationRipper.Get (ref _textToParse);
			return isUpsideCapped;
		}

		#region Properties
		public IDetailRipper<double> UpsideLeverageRipper { get; set; }

		public IDetailRipper<double> MaxReturnRipper { get; set; }

		public IDetailRipper<bool> UpsideIsCappedDeterminationRipper { get; set; }
		#endregion
	}
	#endregion

	#region BufferedPLUSRipper
	public class BufferedPLUSRipper : PLUSRipper
	{
		public virtual double GetBuffer ()
		{
			double buffer = BufferRipper.Get (ref _textToParse);
			return buffer;
		}

		public IDetailRipper<double> BufferRipper { get; set; }
	}
	#endregion

	#region TriggerPLUSRipper
	public class TriggerPLUSRipper : PLUSRipper
	{
		public virtual double GetKnockInBarrier ()
		{
			double kiBarrier = KnockInBarrierRipper.Get (ref _textToParse);
			return kiBarrier;
		}

		public IDetailRipper<double> KnockInBarrierRipper { get; set; }
	}
	#endregion

	#region DualDirectionalTriggerPLUSRipper
	public class DualDirectionalTriggerPLUSRipper : TriggerPLUSRipper
	{
	}
	#endregion

	#region JumpRipper
	public class JumpRipper : DealRipper
	{
		protected PriceOrPercentageRipper _digitalRipper;
		protected IDetailRipper<bool> _upsideIsCappedDeterminationRipper;

		#region Methods
		public virtual decimal GetDigitalPercentage ()
		{
			try
			{
				decimal digital = (decimal)_digitalRipper.Get (ref _textToParse);
				if (_digitalRipper.NumberRepresentsPercentageValue)
				{
					return digital;
				}
				else
				{
					decimal par = (decimal)GetPar ();
					return (digital / par);
				}
			}
			catch
			{
				Debug.WriteLine ("JumpRipper failed with following method: GetDigitalPercentage()");
				throw;
			}
		}

		public virtual bool GetWhetherUpsideIsCapped ()
		{
			bool isUpsideCapped = _upsideIsCappedDeterminationRipper.Get (ref _textToParse);
			return isUpsideCapped;
		}
		#endregion

		#region Properties
		public virtual PriceOrPercentageRipper DigitalRipper
		{
			get { return _digitalRipper; }
			set { _digitalRipper = value; }
		}

		public virtual IDetailRipper<bool> UpsideIsCappedDeterminationRipper
		{
			get { return _upsideIsCappedDeterminationRipper; }
			set { _upsideIsCappedDeterminationRipper = value; }
		}
		#endregion

	}
	#endregion

	#region BufferedJumpRipper
	public class BufferedJumpRipper : JumpRipper
	{
		public virtual double GetBuffer ()
		{
			double buffer = BufferRipper.Get (ref _textToParse);
			return buffer;
		}

		public IDetailRipper<double> BufferRipper { get; set; }
	}
	#endregion

	#region TriggerJumpRipper
	public class TriggerJumpRipper : JumpRipper
	{
		public virtual double GetKnockInBarrier ()
		{
			double kiBarrier = KnockInBarrierRipper.Get (ref _textToParse);
			return kiBarrier;
		}

		public IDetailRipper<double> KnockInBarrierRipper { get; set; }
	}
	#endregion

	#region DualDirectionalTriggerJumpRipper
	public class DualDirectionalTriggerJumpRipper : TriggerJumpRipper
	{
	}
	#endregion

	#region AutocallableRipper
	public class AutocallableRipper : DealRipper
	{
		#region GetCouponPerPeriod
		public virtual decimal GetCouponPerPeriod ()
		{
			decimal couponFromRipper = (decimal)CouponAmountRipper.Get (ref _textToParse);
			if (CouponAmountRipper.CouponIsAnnualized)
			{
				// We assume that if the coupon is annualized, it is expressed as a percentage of par 
				// (and not as a dollar amount).
				CouponFrequency = CouponFrequencyRipper.Get (ref _textToParse);
				int monthsPerPeriod = CouponFrequency.ConvertToNumberOfMonths ();
				decimal couponPerPeriod = couponFromRipper / (12 / monthsPerPeriod);
				return couponPerPeriod;
			}
			if (CouponAmountRipper.NumberRepresentsPercentageValue)
			{
				return couponFromRipper;
			}
			// This condition assumes that because the coupon ripped was not expressed as a % of par 
			// that the coupon amount was not annualized and is the actual coupon amount per period 
			// in dollar terms.
			decimal par = (decimal)GetPar ();
			decimal couponAsPercentageOfPar = couponFromRipper / par;
			return couponAsPercentageOfPar;
		}
		#endregion

		#region GetCouponFrequency
		public virtual string GetCouponFrequency ()
		{
			if (CouponFrequency == null)
			{
				CouponFrequency = CouponFrequencyRipper.Get (ref _textToParse);
				return CouponFrequency;
			}
			else return CouponFrequency;
		}
		#endregion

		#region GetCouponBarrier
		public virtual double GetCouponBarrier ()
		{
			double couponBarrier = CouponBarrierRipper.Get (ref _textToParse);
			return couponBarrier;
		}
		#endregion

		#region GetKnockInBarrier
		public virtual double GetKnockInBarrier ()
		{
			return KnockInBarrierRipper.Get (ref _textToParse);
		}
		#endregion

		#region GetCallObservationDates
		public virtual DateTime [] GetCallObservationDates ()
		{
			return CallObservationDatesRipper.Get (ref _textToParse);
		}
		#endregion

		#region GetCouponObservationDates
		public virtual DateTime [] GetCouponObservationDates ()
		{
			return CouponObservationDatesRipper.Get (ref _textToParse);
		}
		#endregion

		#region Properties
		public IDetailRipper<double> CouponBarrierRipper { get; set; }

		public IDetailRipper<double> KnockInBarrierRipper { get; set; }

		public CouponRipper CouponAmountRipper { get; set; }

		public IDetailRipper<string> CouponFrequencyRipper { get; set; }

		public IDetailRipper<DateTime []> CallObservationDatesRipper { get; set; }

		public IDetailRipper<DateTime []> CouponObservationDatesRipper { get; set; }

		public string CouponFrequency { get; protected set; }
		#endregion
	}
	#endregion

	#region FixedCouponAutocallableRipper
	public class FixedCouponAutocallableRipper : AutocallableRipper
	{
		public override double GetCouponBarrier ()
		{
			return 0;
		}
	}
	#endregion
}
