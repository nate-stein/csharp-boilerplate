using System;

namespace Trinity.DealRipping
{
	public class DealRipperFactoryException : Exception
	{
		public DealRipperFactoryException (string message, Exception innerException) :
			base (message, innerException) { }

		public DealRipperFactoryException (string errorMessage)
			: base (errorMessage)
		{
		}

		public override string ToString ()
		{
			return "DealRipperFactoryException. " + Message;
		}
	}
}
