using System;

namespace Trinity.DealRipping
{
	/// Meant to be thrown whenever an exception is encountered that is not caused by a detail 
	/// ripper failing to parse the desired detail.
	public class DealFactoryException : Exception
	{
		public DealFactoryException (string errorMessage, Exception innerException)
			: base (errorMessage, innerException)
		{
		}

		public DealFactoryException (string errorMessage)
			: base (errorMessage)
		{
		}
	}
}
