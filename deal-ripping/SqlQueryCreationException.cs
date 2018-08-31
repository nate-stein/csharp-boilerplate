using System;

namespace Trinity.DealRipping
{
	/// <summary>
	/// Meant to encapsulate all errors that can arise when we encounter an error creating a Sql 
	/// query to add details to the SP database.
	/// </summary>
	public class SqlQueryCreationException : Exception
	{
		public SqlQueryCreationException () { }

		public SqlQueryCreationException (string errorMessage)
			: base (errorMessage)
		{
		}

		public SqlQueryCreationException (string errorMessage, Exception innerException)
			: base (errorMessage, innerException)
		{
		}

		public override string ToString ()
		{
			return "SqlQueryCreationException. " + Message;
		}
	}
}
