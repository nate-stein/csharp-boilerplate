using System;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Intended to encapsulate all potential exceptions that can occur because of a database sql 
	/// query, setup or connection issues.
	/// </summary>
	public class DatabaseOperationException : Exception
	{
		public DatabaseOperationException () { }

		public DatabaseOperationException (string message) : base (message) { }

		public DatabaseOperationException (string message, Exception innerException) :
			base (message, innerException) { }

		public override string ToString ()
		{
			return "DatabaseOperationException. " + Message;
		}
	}
}
