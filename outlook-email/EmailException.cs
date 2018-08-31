using System;

namespace Trinity.EmailGeneration
{
    #region EmailException
    public class EmailException : Exception
    {
        public EmailException () { }

        public EmailException (string message) : base (message) { }

        public EmailException (string message, Exception innerException) :
            base (message, innerException) { }

        public override string ToString ()
        {
            return "EmailException. " + base.Message;
        }
    }
    #endregion
}
